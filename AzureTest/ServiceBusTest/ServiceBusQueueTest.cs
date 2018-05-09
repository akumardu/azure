using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace AzureTest.ServiceBusTest
{
    [TestClass]
    public class ServiceBusQueueTest
    {
        string queueName = "testqueueamar";
        string connectionString = "Endpoint=sb://testqueueamar.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=HaHY1Vkc3Wrfm9J5xMk16zSRTjuSFxLfxwkO7iguxAQ=";

        [TestMethod]
        public void TestBasicQueueOperations()
        {
            //Send message to queue
            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(connectionString);
            QueueClient queue = factory.CreateQueueClient(queueName);
            string message = "queue message over amqp";
            BrokeredMessage bm = new BrokeredMessage(message);
            queue.Send(bm);


            //Receive from the queue
            BrokeredMessage receivedMessage = queue.Receive();
            if (receivedMessage != null)
            {
                try
                {
                    Console.WriteLine("MessageId {0}", receivedMessage.MessageId);
                    Console.WriteLine("Delivery {0}", receivedMessage.DeliveryCount);
                    Console.WriteLine("Size {0}", receivedMessage.Size);
                    Console.WriteLine(receivedMessage.GetBody<string>());
                    receivedMessage.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    receivedMessage.Abandon();
                }
            }
        }
    }
}
