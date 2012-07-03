using System;
using System.Diagnostics.Contracts;
using System.Net;
using SystemDot.Logging;
using SystemDot.Parallelism;

namespace SystemDot.Http
{
    public class HttpServer : IWorker
    {
        readonly FixedPortAddress address;
        readonly IHttpHandler handler;
        readonly HttpListener listener;
        
        bool isStopped;
        
        public HttpServer(FixedPortAddress address, IHttpHandler handler)
        {
            Contract.Requires(handler != null);

            this.address = address;
            this.handler = handler;
            this.listener = new HttpListener();
        }

        public void StartWork()
        {
            this.listener.Prefixes.Clear();
            this.listener.Prefixes.Add(this.address.Url);
            this.listener.Start();
        }

        public void PerformWork()
        {
            if (this.isStopped || !this.listener.IsListening)
                return;

            var result = listener.BeginGetContext(BeginGetContextCallback, listener);
            result.AsyncWaitHandle.WaitOne();
        }

        private void BeginGetContextCallback(IAsyncResult ar)
        {
            try
            {
                HttpListenerContext context = this.listener.EndGetContext(ar);

                this.handler.HandleRequest(context.Request.InputStream, context.Response.OutputStream);

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.Close();
            }
            catch (HttpListenerException e)
            {
                Logger.Log(e.Message);
            }
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