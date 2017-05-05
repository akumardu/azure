using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTest.ServiceBusTest
{
    public class RelayService : IRelayService
    {
        public string EchoMessage(string message)
        {
            Console.WriteLine(message);
            return "server says: " + message;
        }
    }
}
