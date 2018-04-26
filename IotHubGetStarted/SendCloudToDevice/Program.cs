using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Devices;

namespace SendCloudToDevice
{
    class Program
    {
        static ServiceClient serviceClient;
        static string connectionString = "HostName=demobasichub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=UY4B/i5RhGdRH1mEhMUdjIiGjYhxi69L+5jYmhQZiTs=";
        private static string deviceId = "myFirstDevice";
        private static object _lock = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("Send Cloud-to-Device message\n");
            Thread.Sleep(15000);
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            ReceiveFeedbackAsync();

            //Console.WriteLine("Press any key to send a C2D message.");
            //Console.ReadLine();
            SendCloudToDeviceMessageAsync();
            Console.ReadLine();
        }

        private static async void SendCloudToDeviceMessageAsync()
        {
            while (true)
            {
                var commandMessage = new Message(Encoding.ASCII.GetBytes("Cloud to device message."))
                {
                    Ack = DeliveryAcknowledgement.Full
                };
                try
                {
                    await serviceClient.SendAsync(deviceId, commandMessage);
                    Print("Sending Cloud-to-Device message successfull", ConsoleColor.Yellow);
                }
                catch (Exception ex)
                {
                    Print($"Exception thrown while sending message {ex.Message}\n", ConsoleColor.White);
                }

                await Task.Delay(3000);
            }
        }

        private static async void ReceiveFeedbackAsync()
        {
            var feedbackReceiver = serviceClient.GetFeedbackReceiver();

            Console.WriteLine("\nReceiving c2d feedback from service");
            while (true)
            {
                try
                {
                    var feedbackBatch = await feedbackReceiver.ReceiveAsync();
                    if (feedbackBatch == null) continue;

                    Print($"Received feedback: " + string.Join(",", feedbackBatch.Records.Select(f => f.StatusCode)), ConsoleColor.Green);

                    await feedbackReceiver.CompleteAsync(feedbackBatch);
                }
                catch (Exception ex) 
                {
                    Print($"Exception thrown while receiving feedback {ex.Message}\n", ConsoleColor.White);
                    await Task.Delay(3000);
                }
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
