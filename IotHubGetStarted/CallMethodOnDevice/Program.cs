using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace CallMethodOnDevice
{
    class Program
    {
        static ServiceClient serviceClient;
        static string connectionString = "HostName=testamar.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Fcqxr60RiPqitTZH6dCThuB5yk8TFg5c2Kz3wTCUkDU=";

        static void Main(string[] args)
        {
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            InvokeMethod().Wait();
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }

        private static async Task InvokeMethod()
        {
            var methodInvocation = new CloudToDeviceMethod("writeLine") { ResponseTimeout = TimeSpan.FromSeconds(30) };
            methodInvocation.SetPayloadJson("'a line to be written'");

            var response = await serviceClient.InvokeDeviceMethodAsync("myFirstDevice", methodInvocation);

            Console.WriteLine("Response status: {0}, payload:", response.Status);
            Console.WriteLine(response.GetPayloadAsJson());
        }
    }
}
