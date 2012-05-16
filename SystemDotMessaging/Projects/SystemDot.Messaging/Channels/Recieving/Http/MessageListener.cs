using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace SystemDot.Messaging.Channels.Recieving.Http
{
    public class MessageListener : IMessageListener
    {
        readonly HttpListener listener = new HttpListener();
        Thread listenThread;

        public MessageListener(string listeningOnAddress)
        {
            this.listener = new HttpListener();
            listener.Prefixes.Add(listeningOnAddress);
        }

        public void Start(Action<Object> onMessageRecieved)
        {
            this.listener.Start();
            this.listenThread = new Thread(() => Recieve(onMessageRecieved));
            this.listenThread.Start();
        }

        public void Stop()
        {
            this.listener.Stop();
            this.listenThread.Join();
        }

        void Recieve(Action<object> onMessageRecieved)
        {
            while (true)
            {
                if (!this.listener.IsListening) return;

                HttpListenerContext context = this.listener.GetContext();

                onMessageRecieved.Invoke(DeserialiseMessage(context.Request.InputStream));

                context.Response.StatusCode = (int) HttpStatusCode.OK;
                context.Response.Close();
            }
        }

        static object DeserialiseMessage(Stream toDeserialise)
        {
            return new BinaryFormatter().Deserialize(toDeserialise);
        }
    }
}