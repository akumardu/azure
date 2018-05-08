using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelayWcfService
{
    using System.ServiceModel;

    [ServiceContract]
    public interface IrelayService
    {
        [OperationContract]
        string EchoMessage(string message);

    }

    public class RelayService : IrelayService
    {
        public string EchoMessage(string message)
        {
            Console.WriteLine(message);
            return message;
        }
    }
}
