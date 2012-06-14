using System.Diagnostics.Contracts;
using System.Net;
using SystemDot.Logging;
using SystemDot.Threading;

namespace SystemDot.Http
{
    public class HttpServer : IWorker
    {
        readonly string address;
        readonly IHttpHandler handler;
        readonly HttpListener listener;
        
        bool isStopped;
        
        public HttpServer(string address, IHttpHandler handler)
        {
            Contract.Requires(!string.IsNullOrEmpty(address));
            Contract.Requires(handler != null);

            this.address = address;
            this.handler = handler;
            this.listener = new HttpListener();
        }

        public void StartWork()
        {
            this.listener.Prefixes.Clear();
            this.listener.Prefixes.Add(this.address);
            this.listener.Start();
        }

        public void PerformWork()
        {
            if (this.isStopped || !this.listener.IsListening) 
                return;

            try
            {
                HandleRequest();
            }
            catch (HttpListenerException e)
            {
                Logger.Log(e.Message);
            }
        }

        private void HandleRequest()
        {
            HttpListenerContext context = this.listener.GetContext();

            this.handler.HandleRequest(context.Request.InputStream);
            this.handler.Respond(context.Response.OutputStream);

            context.Response.StatusCode = (int) HttpStatusCode.OK;
            context.Response.Close();
        }

        public void StopWork()
        {
            this.isStopped = true;
            this.listener.Stop();
            this.listener.Prefixes.Clear();
            this.listener.Close();
        }
    }
}