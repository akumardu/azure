using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.Rest;

namespace AzureTest.ServiceBusTest
{
    public static class Relayutils
    {
        public static async Task<string> SetupRelayServer()
        {
            string serviceBusNamespace = ConfigurationManager.AppSettings["ServiceBusNamespace"];
            string listenerPolicyName = ConfigurationManager.AppSettings["ReceiverPolicyName"];
            string listenerPolicyKey = ConfigurationManager.AppSettings["ReceiverPolicyKey"];
            string serviceRelativePath = "Relay";
            ServiceHost host = new ServiceHost(typeof(RelayService));
            host.AddServiceEndpoint(typeof(IRelayService), new NetTcpRelayBinding(), ServiceBusEnvironment.CreateServiceUri("sb", serviceBusNamespace, serviceRelativePath)).Behaviors.Add(new TransportClientEndpointBehavior
            {
                TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(listenerPolicyName, listenerPolicyKey)
            });

            host.Open();
            while (RelayService.MessageReceived == false)
            {
                await Task.Delay(100).ConfigureAwait(false);
            }

            host.Close();
            return RelayService.Message;
        }

        public static string SetupRelayClient(string message)
        {
            string serviceBusNamespace = ConfigurationManager.AppSettings["ServiceBusNamespace"];
            string listenerPolicyName = ConfigurationManager.AppSettings["SenderPolicyName"];
            string listenerPolicyKey = ConfigurationManager.AppSettings["SenderPolicyKey"];
            string serviceRelativePath = "Relay";
            var client = new ChannelFactory<IRelayServiceChannel>(
            new NetTcpRelayBinding(),
            new EndpointAddress(
            ServiceBusEnvironment.CreateServiceUri("sb", serviceBusNamespace, serviceRelativePath)));
            client.Endpoint.Behaviors.Add(
            new TransportClientEndpointBehavior
            {
                TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(listenerPolicyName, listenerPolicyKey)
            });
            using (var channel = client.CreateChannel())
            {
                string messageReturned = channel.EchoMessage(message);
                Console.WriteLine(messageReturned);
                return messageReturned;
            }
        }
    }
}
