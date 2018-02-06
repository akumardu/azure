using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDbTest
{
    [TestClass]
    public class DocumentDbApiTest
    {
        private const string EndpointUrl = "https://localhost:8081";
        private const string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private static DocumentClient documentDbClient;

        private const string DbName = "db";
        private const string CollectionName = "coll";

        [ClassInitialize]
        public static async Task InitializeCosmosDb(TestContext context)
        {
            documentDbClient = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
            await documentDbClient.CreateDatabaseAsync(new Database() { Id = DbName });
            DocumentCollection myCollection = new DocumentCollection();
            myCollection.Id = CollectionName;
            myCollection.PartitionKey.Paths.Add("/deviceId");

            await documentDbClient.CreateDocumentCollectionAsync(
                UriFactory.CreateDatabaseUri(DbName),
                myCollection,
                new RequestOptions { OfferThroughput = 2500 });
        }

        [ClassCleanup]
        public async Task CleanupCosmosDb()
        {
            await documentDbClient.DeleteDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DbName, CollectionName));

            await documentDbClient.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(DbName));
        }

        [TestMethod]
        public async Task TestSQLOnCosmosDB()
        {
            // Create a document. Here the partition key is extracted 
            // as "XMS-0001" based on the collection definition
            await documentDbClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(DbName, CollectionName),
                new DeviceReading
                {
                    Id = "XMS-001-FE24C",
                    DeviceId = "XMS-0001",
                    MetricType = "Temperature",
                    MetricValue = 105.00,
                    Unit = "Fahrenheit",
                    ReadingTime = DateTime.UtcNow
                });

            // Read document. Needs the partition key and the Id to be specified
            Document result = await documentDbClient.ReadDocumentAsync(
              UriFactory.CreateDocumentUri(DbName, CollectionName, "XMS-001-FE24C"),
              new RequestOptions { PartitionKey = new PartitionKey("XMS-0001") });

            DeviceReading reading = (DeviceReading)(dynamic)result;

            // Update the document. Partition key is not required, again extracted from the document
            reading.MetricValue = 104;
            reading.ReadingTime = DateTime.UtcNow;

            await documentDbClient.ReplaceDocumentAsync(
              UriFactory.CreateDocumentUri(DbName, CollectionName, "XMS-001-FE24C"),
              reading);

            // Query using partition key
            IQueryable<DeviceReading> query = documentDbClient.CreateDocumentQuery<DeviceReading>(
                UriFactory.CreateDocumentCollectionUri(DbName, CollectionName))
                .Where(m => m.MetricType == "Temperature" && m.DeviceId == "XMS-0001");
            Assert.AreEqual(query.Count(), 1, $"Invalid query count {query.Count()}");

            // Query across partition keys
            IQueryable<DeviceReading> crossPartitionQuery = documentDbClient.CreateDocumentQuery<DeviceReading>(
                UriFactory.CreateDocumentCollectionUri(DbName, CollectionName),
                new FeedOptions { EnableCrossPartitionQuery = true })
                .Where(m => m.MetricType == "Temperature" && m.MetricValue > 100);
            Assert.AreEqual(crossPartitionQuery.Count(), 1, $"Invalid cross partition query count {crossPartitionQuery.Count()}");

            // Cross-partition Order By queries
            IQueryable<DeviceReading> crossPartitionQueryWithParallelism = documentDbClient.CreateDocumentQuery<DeviceReading>(
                UriFactory.CreateDocumentCollectionUri(DbName, CollectionName),
                new FeedOptions { EnableCrossPartitionQuery = true, MaxDegreeOfParallelism = 10, MaxBufferedItemCount = 100 })
                .Where(m => m.MetricType == "Temperature" && m.MetricValue > 100)
                .OrderBy(m => m.MetricValue);
            Assert.AreEqual(crossPartitionQueryWithParallelism.Count(), 1, $"Invalid cross partition query with parallelism count {crossPartitionQueryWithParallelism.Count()}");

            // Delete a document. The partition key is required.
            await documentDbClient.DeleteDocumentAsync(
              UriFactory.CreateDocumentUri(DbName, CollectionName, "XMS-001-FE24C"),
              new RequestOptions { PartitionKey = new PartitionKey("XMS-0001") });
        }
    }
}
