using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;

namespace ReportConnectivity
{
    class Program
    {
        static string DeviceConnectionString = "HostName=testamar.azure-devices.net;SharedAccessKeyName=iothubowner;DeviceId=myDeviceId;SharedAccessKey=Fcqxr60RiPqitTZH6dCThuB5yk8TFg5c2Kz3wTCUkDU=";
        static DeviceClient Client = null;

        static void Main(string[] args)
        {
            try
            {
                InitClient();
                ReportConnectivity();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error in sample Main: {0}", ex.Message);
            }
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }

        public static async void InitClient()
        {
            try
            {
                Console.WriteLine("Connecting to hub");
                Client = DeviceClient.CreateFromConnectionString(DeviceConnectionString, TransportType.Mqtt);
                Console.WriteLine("Retrieving twin");
                var twin = await Client.GetTwinAsync();
                Console.WriteLine("Twin info: " + twin.ToJson());
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error in sample InitClient: {0}", ex.Message);
            }
        }

        public static async void ReportConnectivity()
        {
            try
            {
                Console.WriteLine("Sending connectivity data as reported property");

                TwinCollection reportedProperties, connectivity;
                reportedProperties = new TwinCollection();
                connectivity = new TwinCollection();
                connectivity["type"] = "cellular";
                reportedProperties["connectivity"] = connectivity;
                await Client.UpdateReportedPropertiesAsync(reportedProperties);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error in sample ReportConnectivity: {0}", ex.Message);
            }
        }
    }
}
