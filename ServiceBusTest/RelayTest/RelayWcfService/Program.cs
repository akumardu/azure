using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelayWcfService
{
    using System.ServiceModel;
    using Microsoft.ServiceBus;

    class Program
    {
        static void Main(string[] args)
        {
            string serviceBusNamespace = "testrelayamar";
            string listenerPolicyName = "RootManageSharedAccessKey";
            string listenerPolicyKey = "1duvGbc8tUq2jGlWMbbEOY/s+aDj8KZeiCq9y6u0PTA=";
            string serviceRelativePath = "testwcfrelay";

            ServiceHost host = new ServiceHost(typeof(RelayService));

            host.AddServiceEndpoint(typeof(IrelayService), new NetTcpRelayBinding()
            {
                IsDynamic = false
            },

               ServiceBusEnvironment.CreateServiceUri("sb", serviceBusNamespace, serviceRelativePath))
               .Behaviors.Add(new TransportClientEndpointBehavior
               {
                   TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(listenerPolicyName, listenerPolicyKey)
               });

            host.Open();

            Console.WriteLine("Service is running. Press ENTER to stop theservice.");
            Console.ReadLine();

            host.Close();
        }
    }
}
