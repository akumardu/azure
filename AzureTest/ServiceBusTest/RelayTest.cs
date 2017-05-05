namespace AzureTest.ServiceBusTest
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.ServiceModel;
    using System.Threading.Tasks;

    using Microsoft.ServiceBus;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RelayTest
    {
        [TestMethod]
        public async Task BasicRelayTest()
        {
            string message = "abracadabra", messageReceived = string.Empty, messageAtServer = string.Empty;
            Task serverTask = new Task(() => { messageAtServer = Relayutils.SetupRelayServer().ConfigureAwait(false).GetAwaiter().GetResult(); });
            serverTask.Start();
            await Task.Delay(30000).ConfigureAwait(false);
            var clientTask = Task.Factory.StartNew(() => messageReceived = Relayutils.SetupRelayClient(message));
            await Task.WhenAll(serverTask, clientTask).ConfigureAwait(false);
            Assert.AreEqual(messageReceived, "server says: " + message, "Comparison failed");
            Assert.AreEqual(message, messageAtServer);
        }
    }
}