using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
//using Microsoft.Azure.Management.Compute.Fluent;
//using Microsoft.Azure.Management.Compute.Fluent.Models;
using System.Threading;
using Microsoft.Azure;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using Microsoft.Rest.Azure.Authentication;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;

namespace AzureManagerTest
{
    class Program
    {
        private static string clientId = "f2a7e58f-681c-4416-83cb-409d0b5dfb01";
        private static string clientSecret = "AmarTest123!"; //yE0A0FGQaGI7D8pebMJNYpSzxMVqv19LWkBFXlGGI/Y=
        private static string tenantId = "a8e776e2-1710-4ed5-8c2f-1a49f83724ec"; 
        private static string resourceGroupName = "myTestResourceGroup";
        private static string availabilitySetName = "myAvailabilitySet";
        private static string publicIpAddressName = "myPublicIpAddress";
        private static string subnetName = "mySubnet";
        private static string networkInterfaceName = "myNetworkInterface";
        private static string vNetName = "myVNet";
        private static string vmName = "amarVmName";
        private static string userName = "amar";
        private static string password = "AmarTest123!";
        private static Region location = Region.USWest;

        static void Main(string[] args)
        {
            var authenticationContext =
                new AuthenticationContext("https://login.windows.net/" + tenantId);
            var credential = new ClientCredential(clientId: clientId, clientSecret: clientSecret);

            var result = authenticationContext.AcquireToken(resource: "https://management.core.windows.net/",
                clientCredential: credential);
            //ComputeManagementClient computeClient =
            //    new ComputeManagementClient(new TokenCloudCredentials("eb0f2b9b-b44c-4019-9a47-6117d9b6cdfa", result.AccessToken));

            //var operationStatus = computeClient.Deployments.RebootRoleInstanceByDeploymentSlotAsync("sampleworkerroleamar", DeploymentSlot.Production, "SampleWorkerRole_IN_0").GetAwaiter().GetResult();
            
            
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }

        //private static void RunBasicAzureManagementTest()
        //{
        //    var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(clientId, clientSecret, tenantId, AzureEnvironment.AzureGlobalCloud);
        //    var azure = Azure.Configure().Authenticate(credentials).WithDefaultSubscription();
        //    Console.WriteLine($"Credentials are valid: {AzureHelper.TestCredentialsAreValid(azure)}");




        //    var resourceGroup = AzureHelper.CreateResourceGroupIfNotExists(azure, resourceGroupName, location);

        //    var availabilitySet = AzureHelper.CreateAvailabilitySetIfNotExists(azure, resourceGroupName, location, availabilitySetName);

        //    var publicIpAddress = AzureHelper.CreatePublicIpIfNotExists(azure, resourceGroupName, location, publicIpAddressName);

        //    var virtualNetwork = AzureHelper.CreateVirtualNetworkIfNotExists(azure, resourceGroupName, location, vNetName);

        //    var networkInterface = AzureHelper.CreateNetworkInterfaceIfNotExists(azure, resourceGroupName, location, virtualNetwork, publicIpAddress, subnetName, networkInterfaceName);

        //    var virtualMachine = AzureHelper.CreateVirtualMachineIfNotExists(azure, resourceGroupName, location, networkInterface, availabilitySet, vmName, userName, password);

        //    PrintVmStatus(virtualMachine);

        //    virtualMachine.RestartAsync();

        //    int i = 0;
        //    while (i < 2)
        //    {
        //        Console.WriteLine($"Counter: {i}");
        //        PrintInstanceStatus(virtualMachine);
        //        Thread.Sleep(10000);
        //        i++;
        //    }
        //}

        //private static void PrintInstanceStatus(IVirtualMachine vm)
        //{
        //    Console.WriteLine("VM general status");
        //    Console.WriteLine("  provisioningStatus: " + vm.ProvisioningState);
        //    Console.WriteLine("  id: " + vm.Id);
        //    Console.WriteLine("  name: " + vm.Name);
        //    Console.WriteLine("  type: " + vm.Type);
        //    Console.WriteLine("  location: " + vm.Region);
        //    Console.WriteLine("VM instance status");
        //    foreach (InstanceViewStatus stat in vm.InstanceView.Statuses)
        //    {
        //        Console.WriteLine("  code: " + stat.Code);
        //        Console.WriteLine("  level: " + stat.Level);
        //        Console.WriteLine("  displayStatus: " + stat.DisplayStatus);
        //    }
        //}

        //static void PrintVmStatus(IVirtualMachine vm)
        //{
        //    Console.WriteLine("Getting information about the virtual machine...");
        //    Console.WriteLine("hardwareProfile");
        //    Console.WriteLine("   vmSize: " + vm.Size);
        //    Console.WriteLine("storageProfile");
        //    Console.WriteLine("  imageReference");
        //    Console.WriteLine("    publisher: " + vm.StorageProfile.ImageReference.Publisher);
        //    Console.WriteLine("    offer: " + vm.StorageProfile.ImageReference.Offer);
        //    Console.WriteLine("    sku: " + vm.StorageProfile.ImageReference.Sku);
        //    Console.WriteLine("    version: " + vm.StorageProfile.ImageReference.Version);
        //    Console.WriteLine("  osDisk");
        //    Console.WriteLine("    osType: " + vm.StorageProfile.OsDisk.OsType);
        //    Console.WriteLine("    name: " + vm.StorageProfile.OsDisk.Name);
        //    Console.WriteLine("    createOption: " + vm.StorageProfile.OsDisk.CreateOption);
        //    Console.WriteLine("    caching: " + vm.StorageProfile.OsDisk.Caching);
        //    Console.WriteLine("osProfile");
        //    Console.WriteLine("  computerName: " + vm.OSProfile.ComputerName);
        //    Console.WriteLine("  adminUsername: " + vm.OSProfile.AdminUsername);
        //    Console.WriteLine("  provisionVMAgent: " + vm.OSProfile.WindowsConfiguration.ProvisionVMAgent.Value);
        //    Console.WriteLine("  enableAutomaticUpdates: " + vm.OSProfile.WindowsConfiguration.EnableAutomaticUpdates.Value);
        //    Console.WriteLine("networkProfile");
        //    foreach (string nicId in vm.NetworkInterfaceIds)
        //    {
        //        Console.WriteLine("  networkInterface id: " + nicId);
        //    }
        //    Console.WriteLine("vmAgent");
        //    Console.WriteLine("  vmAgentVersion" + vm.InstanceView.VmAgent.VmAgentVersion);
        //    Console.WriteLine("    statuses");
        //    foreach (InstanceViewStatus stat in vm.InstanceView.VmAgent.Statuses)
        //    {
        //        Console.WriteLine("    code: " + stat.Code);
        //        Console.WriteLine("    level: " + stat.Level);
        //        Console.WriteLine("    displayStatus: " + stat.DisplayStatus);
        //        Console.WriteLine("    message: " + stat.Message);
        //        Console.WriteLine("    time: " + stat.Time);
        //    }
        //    Console.WriteLine("disks");
        //    foreach (DiskInstanceView disk in vm.InstanceView.Disks)
        //    {
        //        Console.WriteLine("  name: " + disk.Name);
        //        Console.WriteLine("  statuses");
        //        foreach (InstanceViewStatus stat in disk.Statuses)
        //        {
        //            Console.WriteLine("    code: " + stat.Code);
        //            Console.WriteLine("    level: " + stat.Level);
        //            Console.WriteLine("    displayStatus: " + stat.DisplayStatus);
        //            Console.WriteLine("    time: " + stat.Time);
        //        }
        //    }
        //    Console.WriteLine("VM general status");
        //    Console.WriteLine("  provisioningStatus: " + vm.ProvisioningState);
        //    Console.WriteLine("  id: " + vm.Id);
        //    Console.WriteLine("  name: " + vm.Name);
        //    Console.WriteLine("  type: " + vm.Type);
        //    Console.WriteLine("  location: " + vm.Region);
        //    Console.WriteLine("VM instance status");
        //    foreach (InstanceViewStatus stat in vm.InstanceView.Statuses)
        //    {
        //        Console.WriteLine("  code: " + stat.Code);
        //        Console.WriteLine("  level: " + stat.Level);
        //        Console.WriteLine("  displayStatus: " + stat.DisplayStatus);
        //    }
            
        //}

        
    }
}
