using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;

namespace AzureManagerTest
{
    class Program
    {
        private static string clientId = "f2a7e58f-681c-4416-83cb-409d0b5dfb01";
        private static string clientSecret = "AmarTest123!";
        private static string tenantId = "a8e776e2-1710-4ed5-8c2f-1a49f83724ec";

        static void Main(string[] args)
        {
            var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(clientId, clientSecret, tenantId, AzureEnvironment.AzureGlobalCloud);
            var azure = Azure.Configure().Authenticate(credentials).WithDefaultSubscription();
            TestCredentialsAreValid(azure);
        }

        static bool TestCredentialsAreValid(IAzure azure)
        {
            return azure.Subscriptions.List().Count() == 1;
        }

        static void TestVmCrudAndRestart(IAzure azure)
        {
            var vmList = azure.VirtualMachines.List();
            foreach (var vm in vmList)
            {
                Console.WriteLine("VM Name: {0}", vm.Name);
            }
        }
    }
}
