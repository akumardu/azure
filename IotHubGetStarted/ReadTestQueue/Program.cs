using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Microsoft.ServiceBus.Messaging;

namespace ReadTestQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Receive critical messages. Ctrl-C to exit.\n");
            var connectionString = "Endpoint=sb://testamar.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=BPm20sdfTWEDJ9a93DdEQbwe60kRMGf+kHdUP9aWRuQ=";
            var queueName = "queue1";

            var client = QueueClient.CreateFromConnectionString(connectionString, queueName);

            client.OnMessage(message =>
            {
                Stream stream = message.GetBody<Stream>();
                StreamReader reader = new StreamReader(stream, Encoding.ASCII);
                string s = reader.ReadToEnd();
                Console.WriteLine(String.Format("Message body: {0}", s));
            });

            Console.ReadLine();
        }
    }
}
