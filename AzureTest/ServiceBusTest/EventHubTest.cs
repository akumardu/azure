using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

namespace AzureTest.ServiceBusTest
{
    [TestClass]
    public class EventHubTest
    {
        string eventHubName = "testeventhubamar";
        string connectionString = "Endpoint=sb://testeventhubamar.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Gd1AU/ULRvrtr9c7JV6dsb4OSdpgInEpfPkoGV9Vywk=;TransportType=Amqp";

        [TestMethod]
        public void TestBasicEventHubOperations()
        {
            // Send to event hub
            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(connectionString);

            EventHubClient client = factory.CreateEventHubClient(eventHubName);
            string message = "event hub message";
            EventData data = new EventData(Encoding.UTF8.GetBytes(message));
            client.Send(data);

            // Receive messages via Consumer Groups
            EventHubConsumerGroup group = client.GetDefaultConsumerGroup();
            EventHubReceiver receiver = group.CreateReceiver("0");

            EventData receiveData = receiver.Receive();

            if (data != null)
            {
                try
                {
                    string receivedMessage = Encoding.UTF8.GetString(data.GetBytes());
                    Console.WriteLine("EnqueuedTimeUtc: {0}", data.EnqueuedTimeUtc);
                    Console.WriteLine("PartitionKey: {0}", data.PartitionKey);
                    Console.WriteLine("SequenceNumber: {0}", data.SequenceNumber);
                    Console.WriteLine(receivedMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
