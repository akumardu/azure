using System;

namespace AzureTest.Storage
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.Search;
    using Microsoft.Azure.Search.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AzureSearchTest
    {
        [TestMethod]
        public void TestIndexCreation()
        {
            string searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
            string apiKey = ConfigurationManager.AppSettings["SearchKey"];
            var searchServiceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
            if(searchServiceClient.Indexes.Exists("hotels"))
            {
                searchServiceClient.Indexes.Delete("hotels");
                Assert.IsTrue(searchServiceClient.Indexes.Exists("hotels") == false, "Indexes not deleted");
            }

            var indexDefinition = new Index()
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
            string searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
            string apiKey = ConfigurationManager.AppSettings["SearchKey"];
            var searchServiceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
            if (searchServiceClient.Indexes.Exists("hotels"))
            {
                searchServiceClient.Indexes.Delete("hotels");
            }
        }
    }
}
