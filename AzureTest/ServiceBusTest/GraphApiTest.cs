namespace AzureTest.ServiceBusTest
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Azure.ActiveDirectory.GraphClient;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GraphApiTest
    {
        // Help from https://code.msdn.microsoft.com/Azure-Active-Directory-f7113525/sourcecode?fileId=137449&pathId=921555989
        // In the App registrations blade, click Add, and enter a friendly name for the application, 
        // for example "Console App for Azure AD", select Native, add use https://localhost/ as the Redirect URI. Click Create.

        private string graphUrl = ConfigurationManager.AppSettings["GraphUrl"];

        private string tenantId = ConfigurationManager.AppSettings["TenantId"];

        private string loginUrl = ConfigurationManager.AppSettings["LoginUrl"];

        private string clientId = ConfigurationManager.AppSettings["ClientId"];

        private string clientSecret = ConfigurationManager.AppSettings["ClientSecret"];

        [TestMethod]
        public void TestBasicQueryOnGraphApi()
        {
            string serviceRoot = this.graphUrl + "/" + this.tenantId;
            var graphClient = new ActiveDirectoryClient(new Uri(serviceRoot), async () => await this.GetTokenAsync().ConfigureAwait(false));
            List<IUser> users = graphClient.Users.ExecuteAsync().Result.CurrentPage.ToList();
            Assert.IsTrue(users.Count > 0);
        }

        private async Task<string> GetTokenAsync()
        {
            AuthenticationContext authContext = new AuthenticationContext(this.loginUrl, false);
            ClientCredential creds = new ClientCredential(this.clientId, this.clientSecret);

            var result = await authContext.AcquireTokenAsync(this.graphUrl, creds).ConfigureAwait(false);
            
            Console.WriteLine($"{result.AccessTokenType}-{result.AccessToken}");
            return result.AccessToken;
        }
    }
}