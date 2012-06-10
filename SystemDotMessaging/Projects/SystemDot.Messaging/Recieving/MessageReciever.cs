using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.Pipes;
using SystemDot.Threading;

namespace SystemDot.Messaging.Recieving
{
    public class MessageReciever : IWorker
    {
        readonly HttpListener listener;
        readonly IPipe pipe;
        bool isStopped;

        public MessageReciever(IPipe pipe, string address)
        {
            Contract.Requires(pipe != null);
            Contract.Requires(!string.IsNullOrEmpty(address));
            
            this.pipe = pipe;
            this.listener = new HttpListener();
            this.listener.Prefixes.Add(address);
        }

        public void OnWorkStarted()
        {
            this.listener.Start();
        }

        public void PerformWork()
        {
            if (this.isStopped) return;
            if (!this.listener.IsListening) return;

            try
            {
                HttpListenerContext context = this.listener.GetContext();

                this.pipe.Publish(DeserialiseMessage(context.Request.InputStream));

                context.Response.StatusCode = (int) HttpStatusCode.OK;
                context.Response.Close();
            }
            catch(HttpListenerException) {}
        }

        public void OnWorkStopped()
        {
            this.isStopped = true;
        }

        static object DeserialiseMessage(Stream toDeserialise)
        {
            return new BinaryFormatter().Deserialize(toDeserialise);
        }
    }
}