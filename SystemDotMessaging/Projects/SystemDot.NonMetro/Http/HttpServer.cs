using System.Diagnostics.Contracts;
using System.Net;
using System.Threading.Tasks;
using SystemDot.Logging;

namespace SystemDot.Http
{
    public class HttpServer
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
                context.Response.Close();
            }
            catch (HttpListenerException e)
            {
                Logger.Info(e.Message);
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