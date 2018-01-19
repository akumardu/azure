using System.Configuration;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureTest.Storage
{
    [TestClass]
    public class AzureSearchTest
    {
        [TestMethod]
        public void TestIndexCreation()
        {
            var searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
            var apiKey = ConfigurationManager.AppSettings["SearchKey"];
            var searchServiceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
            if (searchServiceClient.Indexes.Exists("hotels"))
            {
                searchServiceClient.Indexes.Delete("hotels");
                Assert.IsTrue(searchServiceClient.Indexes.Exists("hotels") == false, "Indexes not deleted");
            }

            var indexDefinition = new Index
            {
                Name = "hotels",
                Fields = FieldBuilder.BuildForType<Hotel>()
            };

            searchServiceClient.Indexes.Create(indexDefinition);
            Assert.IsTrue(searchServiceClient.Indexes.Exists("hotels"), "Index creation failed");
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            var searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
            var apiKey = ConfigurationManager.AppSettings["SearchKey"];
            var searchServiceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
            if (searchServiceClient.Indexes.Exists("hotels"))
            {
                searchServiceClient.Indexes.Delete("hotels");
            }
        }
    }
}