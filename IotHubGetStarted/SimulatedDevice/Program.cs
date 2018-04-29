using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimulatedDevice
{
    class Program
    {
        static DeviceClient deviceClient;
        static string iotHubUri = "demobasichub.azure-devices.net";
        static string deviceKey = "bShnwZR77QzL+YZmWWzQmdQVXRVzJSWnajib1clSzvI=";
        private static string deviceId = "myFirstDevice";
        static object _lock = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("Simulated device\n");
            Thread.Sleep(15000);
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey), TransportType.Mqtt);

            deviceClient.ProductInfo = "HappyPath_Simulated-CSharp";

            ReceiveC2dAsync();
            Thread.Sleep(5000);
            SendDeviceToCloudMessagesRoutingAsync();
            
            //SendToBlobAsync();

            // setup callback for "writeLine" method
            //deviceClient.SetMethodHandlerAsync("writeLine", WriteLineToConsole, null).Wait();
            //Console.WriteLine("Waiting for direct method call\n Press enter to exit.");
            Console.ReadLine();

            Console.WriteLine("Exiting...");

            // as a good practice, remove the "writeLine" handler
            //deviceClient.SetMethodHandlerAsync("writeLine", null, null).Wait();
            //Console.ReadLine();
        }

        static Task<MethodResponse> WriteLineToConsole(MethodRequest methodRequest, object userContext)
        {
            Console.WriteLine();
            Console.WriteLine("\t{0}", methodRequest.DataAsJson);
            Console.WriteLine("\nReturning response for method {0}", methodRequest.Name);

            string result = "'Input was written to log.'";
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 200));
        }

        private static async void SendToBlobAsync()
        {
            string fileName = "Sketch.png";
            Console.WriteLine("Uploading file: {0}", fileName);
            var watch = System.Diagnostics.Stopwatch.StartNew();

            using (var sourceData = new FileStream(@"Sketch.png", FileMode.Open))
            {
                await deviceClient.UploadToBlobAsync(fileName, sourceData);
            }

            watch.Stop();
            Console.WriteLine("Time to upload file: {0}ms\n", watch.ElapsedMilliseconds);
        }

        private static async void ReceiveC2dAsync()
        {
            Print("\nSetup to receive cloud to device messages from service", ConsoleColor.Yellow);
            while (true)
            {
                Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;

                Print($"Received message: {Encoding.ASCII.GetString(receivedMessage.GetBytes())}", ConsoleColor.Yellow);
                await deviceClient.CompleteAsync(receivedMessage);
            }
        }

        private static async void SendDeviceToCloudMessagesRoutingAsync()
        {
            double minTemperature = 20;
            double minHumidity = 60;
            Random rand = new Random();

            while (true)
            {
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;

                var telemetryDataPoint = new
                {
                    deviceId = "myFirstDevice",
                    temperature = currentTemperature,
                    humidity = currentHumidity
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                string levelValue;

                if (rand.NextDouble() > 0.7)
                {
                    if (rand.NextDouble() > 0.5)
                    {
                        messageString = "This is a critical message";
                        levelValue = "critical";
                    }
                    else
                    {
                        messageString = "This is a storage message";
                        levelValue = "storage";
                    }
                }
                else
                {
                    levelValue = "normal";
                }

                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                message.Properties.Add("level", levelValue);

                await deviceClient.SendEventAsync(message);
                Print($"{DateTime.Now} > Sending message: {messageString}", ConsoleColor.Blue);

                await Task.Delay(2000);
            }
        }

        private static async void SendDeviceToCloudMessagesAsync()
        {
            double minTemperature = 20;
            double minHumidity = 60;
            int messageId = 1;
            Random rand = new Random();

            while (true)
            {
                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;

                var telemetryDataPoint = new
                {
                    messageId = messageId++,
                    deviceId = "myFirstDevice",
                    temperature = currentTemperature,
                    humidity = currentHumidity
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));
                message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                await deviceClient.SendEventAsync(message);
                Print($"{DateTime.Now} > Sending message: {messageString}", ConsoleColor.Blue);

                await Task.Delay(1000);
            }
        }

        private static void Print(string message, ConsoleColor color)
        {
            lock (_lock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}
