using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AzureTest.ServiceBusTest
{
    [ServiceContract]
    public interface IRelayService
    {
        [OperationContract]
        string EchoMessage(string message);
    }

    public interface IRelayServiceChannel : IRelayService, IClientChannel { }
}
