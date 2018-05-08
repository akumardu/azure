using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelayWcfClient
{
    using Microsoft.ServiceBus;
    using System.ServiceModel;

    class Program {
        static void Main(string[] args)
        {
            string serviceBusNamespace = "testrelayamar";
            string senderPolicyName = "RootManageSharedAccessKey";
            string senderPolicyKey = "1duvGbc8tUq2jGlWMbbEOY/s+aDj8KZeiCq9y6u0PTA=";
            string serviceRelativePath = "testwcfrelay";

            var client = new ChannelFactory<IrelayServiceChannel>(new NetTcpRelayBinding()
            {
                IsDynamic = false
            }, 
            new EndpointAddress(ServiceBusEnvironment.CreateServiceUri("sb", serviceBusNamespace, serviceRelativePath)));

            client.Endpoint.Behaviors.Add(new TransportClientEndpointBehavior
            {
                TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(senderPolicyName, senderPolicyKey)
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
