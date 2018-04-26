//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using Microsoft.Azure.Management.Fluent;
//using Microsoft.Azure.Management.ResourceManager.Fluent;
//using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
//using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
//using Microsoft.Azure.Management.Compute.Fluent.Models;
//using Microsoft.Azure.Management.Compute.Fluent;
//using Microsoft.Azure.Management.Network.Fluent;


//namespace AzureManagerTest
//{
//    public static class AzureHelper
//    {
//        public static IResourceGroup CreateResourceGroupIfNotExists(IAzure azure, string groupName, Region region)
//        {
//            var resourceGroups = azure.ResourceGroups.List();
//            if (resourceGroups.Any(rg => rg.Name.Equals(groupName)))
//            {
//                Console.WriteLine("Resource Group already exists");
//                return resourceGroups.SingleOrDefault(rg => rg.Name.Equals(groupName));
//            }

//            Console.WriteLine("Creating resource group...");
//            var resourceGroup = azure.ResourceGroups.Define(groupName)
//                .WithRegion(region)
//                .Create();

//            return resourceGroup;
//        }

//        public static IAvailabilitySet CreateAvailabilitySetIfNotExists(IAzure azure, string groupName, Region location, string availabiltiySetName)
//        {
//            var availabilitySets = azure.AvailabilitySets.List();
//            if (availabilitySets.Any(av => av.Name.Equals(availabiltiySetName)))
//            {
//                Console.WriteLine("Availability Set already exists");
//                return availabilitySets.SingleOrDefault(av => av.Name.Equals(availabiltiySetName));
//            }

//            Console.WriteLine("Creating availability set...");
//            var availabilitySet = azure.AvailabilitySets.Define(availabiltiySetName)
//                .WithRegion(location)
//                .WithExistingResourceGroup(groupName)
//                .WithSku(AvailabilitySetSkuTypes.Managed)
//                .Create();

//            return availabilitySet;
//        }

//        public static IPublicIPAddress CreatePublicIpIfNotExists(IAzure azure, string groupName, Region location, string publicIpAddressName)
//        {
//            var publicIpAddresses = azure.PublicIPAddresses.List();
//            if (publicIpAddresses.Any(pip => pip.Name.Equals(publicIpAddressName)))
//            {
//                Console.WriteLine("Public Ip Address already exists");
//                return publicIpAddresses.SingleOrDefault(pip => pip.Name.Equals(publicIpAddressName));
//            }

//            Console.WriteLine("Creating public IP address...");
//            var publicIPAddress = azure.PublicIPAddresses.Define(publicIpAddressName)
//                .WithRegion(location)
//                .WithExistingResourceGroup(groupName)
//                .WithDynamicIP()
//                .Create();

//            return publicIPAddress;
//        }

//        public static INetwork CreateVirtualNetworkIfNotExists(IAzure azure, string groupName, Region location, string vNetName)
//        {
//            var vNetList = azure.Networks.List();
//            if (vNetList.Any(vnet => vnet.Name.Equals(vNetName)))
//            {
//                Console.WriteLine("Virtual Network already exists");
//                return vNetList.SingleOrDefault(vnet => vnet.Name.Equals(vNetName));
//            }

//            Console.WriteLine("Creating virtual network...");
//            var network = azure.Networks.Define(vNetName)
//                .WithRegion(location)
//                .WithExistingResourceGroup(groupName)
//                .WithAddressSpace("10.0.0.0/16")
//                .WithSubnet("mySubnet", "10.0.0.0/24")
//                .Create();

//            return network;
//        }

//        public static INetworkInterface CreateNetworkInterfaceIfNotExists(IAzure azure, string groupName, Region location, INetwork network, IPublicIPAddress publicIPAddress, string subnetName, string networkInterfaceName)
//        {
//            var networkInterfaces = azure.NetworkInterfaces.List();
//            if (networkInterfaces.Any(ni => ni.Name.Equals(networkInterfaceName)))
//            {
//                Console.WriteLine("Network Interfaces already exists");
//                return networkInterfaces.SingleOrDefault(ni => ni.Name.Equals(networkInterfaceName));
//            }

//            Console.WriteLine("Creating network interface...");
//            var networkInterface = azure.NetworkInterfaces.Define(networkInterfaceName)
//                .WithRegion(location)
//                .WithExistingResourceGroup(groupName)
//                .WithExistingPrimaryNetwork(network)
//                .WithSubnet(subnetName)
//                .WithPrimaryPrivateIPAddressDynamic()
//                .WithExistingPrimaryPublicIPAddress(publicIPAddress)
//                .Create();

//            return networkInterface;
//        }

//        public static bool TestCredentialsAreValid(IAzure azure)
//        {
//            return azure.Subscriptions.List().Count() == 1;
//        }

//        public static IVirtualMachine CreateVirtualMachineIfNotExists(IAzure azure, string groupName, Region location, INetworkInterface networkInterface, IAvailabilitySet availabilitySet, string vmName, string userName, string password)
//        {
//            var vmList = azure.VirtualMachines.List();
//            if (vmList.Any(vm => vm.Name.Equals(vmName)))
//            {
//                Console.WriteLine("VM already exists");
//                return vmList.SingleOrDefault(pip => pip.Name.Equals(vmName));
//            }

//            Console.WriteLine("Creating virtual machine...");
//            var virtualMachine = azure.VirtualMachines.Define(vmName)
//                .WithRegion(location)
//                .WithExistingResourceGroup(groupName)
//                .WithExistingPrimaryNetworkInterface(networkInterface)
//                .WithLatestWindowsImage("MicrosoftWindowsServer", "WindowsServer", "2012-R2-Datacenter")
//                .WithAdminUsername(userName)
//                .WithAdminPassword(password)
//                .WithComputerName(vmName)
//                .WithExistingAvailabilitySet(availabilitySet)
//                .WithSize(VirtualMachineSizeTypes.StandardDS1)
//                .Create();

//            return virtualMachine;
//        }
//    }
//}
