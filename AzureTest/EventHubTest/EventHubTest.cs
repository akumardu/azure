﻿
namespace AzureTest.EventHubTest
{
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [TestClass]
    public class EventHubTest
    {
        private const string ConnectionString = "Endpoint=sb://testamareventhub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=eRSbbGaZfW685cC4ovv/MV6BHFYkZY7SLRITctdwmmM=";
        private const string EventHubName = "testhub";

        private const string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=demostorewilliam;AccountKey=qU971XsnHHzCTKI58z00QHKPxL7/uf8F+LhWORZIKUcUTnYn7CKV4QdBrfVOzsSszbJPTIA+EzyVCA1iej8JSQ==;EndpointSuffix=core.windows.net";

        [TestMethod]
        public async Task BasicEventHubReadFromReceiverTest()
        {
            var eventHubClient = EventHubClient.CreateFromConnectionString(ConnectionString, EventHubName);

            await SendMessagesToEventHub(eventHubClient, 100);
       
            var count = await ReceiveMessage(eventHubClient);

            Assert.AreEqual(count, 100, $"{count} doesn't match");
        }

        [TestMethod]
        public async Task BasicEventHubReadUsingProcessorTest()
        {
            var eventHubClient = EventHubClient.CreateFromConnectionString(ConnectionString, EventHubName);

            string eventProcessorHostName = "Fixed-EventProcessorName";
            EventProcessorHost eventProcessorHost = new EventProcessorHost(eventProcessorHostName, EventHubName, EventHubConsumerGroup.DefaultGroupName, ConnectionString, StorageConnectionString);
            Console.WriteLine("Registering EventProcessor...");
            var options = new EventProcessorOptions();
            options.ExceptionReceived += (sender, e) => { Console.WriteLine(e.Exception); };
            eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>(options).Wait();


            await SendMessagesToEventHub(eventHubClient, 100);
            await Task.Delay(50000);
            
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();

            int count = SimpleEventProcessor.messageReceived;
            Assert.AreEqual(100, count, $"{count} doesn't match");
        }

        private static async Task<int> ReceiveMessage(EventHubClient eventHubClient)
        {
            var d2cPartitions = (await eventHubClient.GetRuntimeInformationAsync()).PartitionIds;

            var tasks = new List<Task<int>>();
            foreach (string partition in d2cPartitions)
            {
                tasks.Add(ReceiveMessageFromEventHub(eventHubClient, partition));
            }
            Task.WaitAll(tasks.ToArray());

            return tasks.Select(x => x.GetAwaiter().GetResult()).Sum();
        }

        private static async Task<int> ReceiveMessageFromEventHub(EventHubClient eventHubClient, string partition)
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow.AddMinutes(-2));
            int count = 0;
            for(int i = 0; i < 1000; i++)
            {
                var eventData = await eventHubReceiver.ReceiveAsync(1, TimeSpan.FromMilliseconds(100));
                if (eventData == null) continue;

                var data = Encoding.UTF8.GetString(eventData.First().GetBytes());
                Console.WriteLine("Message received. Partition: {0} Data: '{1}'", partition, data);
                count++;
            }

            return count;
        }

        private static async Task SendMessagesToEventHub(EventHubClient eventHubClient, int numMessagesToSend)
        {
            for (var i = 0; i < numMessagesToSend; i++)
            {
                try
                {
                    var message = $"Message {i}";
                    Console.WriteLine($"Sending message: {message}");

                    //     Use this method to send if:
                    //     a) the Microsoft.Azure.EventHubs.EventHubClient.SendAsync(Microsoft.Azure.EventHubs.EventData)
                    //     operation should be highly available and
                    //     b) the data needs to be evenly distributed among all partitions; exception being,
                    //     when a subset of partitions are unavailable
                    //     Microsoft.Azure.EventHubs.EventHubClient.SendAsync(Microsoft.Azure.EventHubs.EventData)
                    //     sends the Microsoft.Azure.EventHubs.EventData to a Service Gateway, which in-turn
                    //     will forward the EventData to one of the EventHub's partitions. Here's the message
                    //     forwarding algorithm:
                    //     i. Forward the EventDatas to EventHub partitions, by equally distributing the
                    //     data among all partitions (ex: Round-robin the EventDatas to all EventHub partitions)
                    //     ii. If one of the EventHub partitions is unavailable for a moment, the Service
                    //     Gateway will automatically detect it and forward the message to another available
                    //     partition - making the send operation highly-available.
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }

                await Task.Delay(10);
            }

            Console.WriteLine($"{numMessagesToSend} messages sent.");
        }
    }
}
