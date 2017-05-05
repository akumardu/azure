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
        public static void SetupRelayServer()
        {
            string serviceBusNamespace = ConfigurationManager.AppSettings[""];
            string listenerPolicyName = "";
            string listenerPolicyKey = "";
            string serviceRelativePath = "";
            ServiceHost host = new ServiceHost(typeof(RelayService));
            host.AddServiceEndpoint(typeof(IRelayService), new NetTcpRelayBinding(), ServiceBusEnvironment.CreateServiceUri("sb", serviceBusNamespace, serviceRelativePath)).Behaviors.Add(new TransportClientEndpointBehavior
            {
                TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(listenerPolicyName, listenerPolicyKey)
            });

            host.Open();
            Console.WriteLine("Service is running.Press ENTER to stop the service.");
            Console.ReadLine();
            host.Close();
        }

        public static void SetupRelayClient()
        {
            string serviceBusNamespace = "";
            string listenerPolicyName = "";
            string listenerPolicyKey = "";
            string serviceRelativePath = "";
            var client = new ChannelFactory<IRelayServiceChannel>(
            new NetTcpRelayBinding(),
            new EndpointAddress(
            ServiceBusEnvironment.CreateServiceUri("sb",
            serviceBusNamespace, serviceRelativePath)));
            client.Endpoint.Behaviors.Add(
            new TransportClientEndpointBehavior
            {
                TokenProvider =
            TokenProvider.CreateSharedAccessSignatureTokenProvider(listenerPolicyName,
            listenerPolicyKey)
            });
            using (var channel = client.CreateChannel())
            {
                string message = channel.EchoMessage("hello from the relay!");
                Console.WriteLine(message);
            }
            Console.ReadLine();
        }
    }
}
