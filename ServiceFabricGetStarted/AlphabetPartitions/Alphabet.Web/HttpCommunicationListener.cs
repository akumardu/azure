using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using System.Fabric.Description;

namespace Alphabet.Web
{
    internal class HttpCommunicationListener : ICommunicationListener
    {
        private string uriPrefix;
        private string uriPublished;
        private Func<HttpListenerContext, CancellationToken, Task> processInternalRequest;
        private HttpListener httpListener;

        public HttpCommunicationListener(string uriPrefix, string uriPublished, Func<HttpListenerContext, CancellationToken, Task> processInternalRequest)
        {
            this.uriPrefix = uriPrefix;
            this.uriPublished = uriPublished;
            this.processInternalRequest = processInternalRequest;
        }

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            this.httpListener = new HttpListener();
            this.httpListener.Prefixes.Add(uriPrefix);
            this.httpListener.Start();

            return Task.FromResult(uriPublished);
        }
    }
}