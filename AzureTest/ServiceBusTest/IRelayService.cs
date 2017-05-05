using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AzureTest.ServiceBusTest
{
    [ServiceContract]
    interface IRelayService
    {
        [OperationContract]
        string EchoMessage(string message);
    }
}
