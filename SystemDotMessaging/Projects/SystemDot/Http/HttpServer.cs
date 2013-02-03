using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Threading.Tasks;

namespace SystemDot.Http
{
    public class HttpServer
    {
        readonly FixedPortAddress address;
        readonly IHttpHandler handler;
        readonly HttpListener listener;
        
        public HttpServer(FixedPortAddress address, IHttpHandler handler)
        {
            Contract.Requires(handler != null);

            this.address = address;
            this.handler = handler;
            this.listener = new HttpListener();
        }

        public void Start()
        {
            this.listener.Prefixes.Clear();
            this.listener.Prefixes.Add(this.address.Url);
            this.listener.Start();

            Task.Factory.StartNew(PerformWork);
        }

        public void PerformWork()
        {    
            Task<HttpListenerContext> context = Task.Factory.FromAsync<HttpListenerContext>(
                this.listener.BeginGetContext, 
                this.listener.EndGetContext, 
                this.listener);

            context.ContinueWith(_ => PerformWork());
            context.ContinueWith(task => HandleRequest(task.Result));            
        }

        private void HandleRequest(HttpListenerContext context)
        {
            try
            {
                this.handler.HandleRequest(context.Request.InputStream, context.Response.OutputStream);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            finally
            {
                context.Response.Close();
            }
        }

        public void StopWork()
        {
            this.listener.Stop();
            this.listener.Prefixes.Clear();
            this.listener.Close();
        }
    }
}