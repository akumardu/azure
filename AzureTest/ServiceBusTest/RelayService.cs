using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTest.ServiceBusTest
{
    public class RelayService : IRelayService
    {
        public static bool MessageReceived = false;

        public static string Message = string.Empty;

        public string EchoMessage(string message)
        {
            Console.WriteLine(message);
            MessageReceived = true;
            Message = message;
            return "server says: " + message;
        }
    }
}
