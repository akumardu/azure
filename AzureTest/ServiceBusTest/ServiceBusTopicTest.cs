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
    public class ServiceBusTopicTest
    {
        string topicName = "testtopicamar";
        string subscriptionName = "testsubscriptionamar";
        string connectionString = "Endpoint=sb://testqueueamar.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=HaHY1Vkc3Wrfm9J5xMk16zSRTjuSFxLfxwkO7iguxAQ=";

        [TestMethod]
        public void TestBasicQueueOperations()
        {
            // Send message to topic
            MessagingFactory factory = MessagingFactory.CreateFromConnectionString(connectionString);

            TopicClient topic = factory.CreateTopicClient(topicName);
            topic.Send(new BrokeredMessage("topic message"));


            // Receive message from subscription
            SubscriptionClient clientA = factory.CreateSubscriptionClient(topicName, subscriptionName);

            BrokeredMessage message = clientA.Receive();
            if (message != null)
            {
                try
                {
                    Console.WriteLine("MessageId {0}", message.MessageId);
                    Console.WriteLine("Delivery {0}", message.DeliveryCount);
                    Console.WriteLine("Size {0}", message.Size);
                    Console.WriteLine(message.GetBody<string>());
                    message.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    message.Abandon();
                }
            }
        }
    }
}
