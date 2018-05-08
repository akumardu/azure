using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelayWcfClient
{
    using System.ServiceModel;

    [ServiceContract]
    public interface IrelayService
    {
        [OperationContract] string EchoMessage(string message);
    }

    public interface IrelayServiceChannel : IrelayService, IClientChannel { }
}
