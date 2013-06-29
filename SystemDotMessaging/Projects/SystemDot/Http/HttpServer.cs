using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Threading.Tasks;
using SystemDot.Logging;

namespace SystemDot.Http
{
    public class HttpServer : IHttpServer
    {
        readonly FixedPortAddress address;
        readonly IHttpHandler handler;
        readonly HttpListener listener;
        
        public HttpServer(FixedPortAddress address, IHttpHandler handler)
        {
            Contract.Requires(handler != null);

            this.address = address;
            this.handler = handler;
            listener = new HttpListener();
        }

        public void Start()
        {
            Logger.Info("HTTP server listening on: {0}", address.Url);

            listener.Prefixes.Clear();
            listener.Prefixes.Add(address.Url);
            listener.Start();

            Task.Factory.StartNew(PerformWork);
        }

        public void PerformWork()
        {    
            Task<HttpListenerContext> context = Task.Factory.FromAsync<HttpListenerContext>(
                listener.BeginGetContext, 
                listener.EndGetContext, 
                listener);

            context.ContinueWith(_ => PerformWork());
            context.ContinueWith(task => HandleRequest(task.Result));            
        }

        private void HandleRequest(HttpListenerContext context)
        {
            try
            {
                handler.HandleRequest(context.Request.InputStream, context.Response.OutputStream);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                Logger.Info("Http server: Bad request: {0}", e.Message);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            finally
            {
                context.Response.Close();
            }
        }

        public void StopWork()
        {
            listener.Stop();
            listener.Prefixes.Clear();
            listener.Close();
        }
    }
}