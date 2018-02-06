using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDbTest
{
    [TestClass]
    public class MongoDbApiTest
    {
        private static MongoClient mongoClient;

        private const string MongoConnectionString = "mongodb://localhost:C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==@localhost:10255/admin?ssl=true";

        private const string DbName = "db";
        private const string CollectionName = "coll";

        
        [ClassInitialize]
        public static async Task InitializeCosmosDb(TestContext testContext)
        {
            mongoClient = new MongoClient(MongoConnectionString);
            var database = mongoClient.GetDatabase(DbName);

            await database.CreateCollectionAsync(CollectionName);
        }

        [ClassCleanup]
        public async Task CleanupCosmosDb()
        {
            var database = mongoClient.GetDatabase(DbName);
            await database.DropCollectionAsync(CollectionName);

            await mongoClient.DropDatabaseAsync(DbName);
        }

        [TestMethod]
        public async Task TestMongoOnCosmosDB()
        {
            var database = mongoClient.GetDatabase(DbName);
            var todoTaskCollection = database.GetCollection<MyTask>(CollectionName);

            // Create a document. Here the partition key is extracted 
            // as "XMS-0001" based on the collection definition
            await todoTaskCollection.InsertOneAsync(
                new MyTask
                {
                    Name = "Do Something",
                    Category = "Work",
                    CreatedDate = DateTime.UtcNow,
                    Date = DateTime.UtcNow
                });

            // Read document. 
            FilterDefinition<MyTask> filter = FilterDefinition<MyTask>.Empty;
            var result = await todoTaskCollection.FindSync(filter).ToListAsync();

            Assert.IsTrue(result.Count() == 1, $"Invalid result count {result.Count()}");

            await todoTaskCollection.DeleteOneAsync(filter);

            
        }

    }
}
