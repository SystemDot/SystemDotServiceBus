using System.IO;
using SystemDot.Http;

namespace SystemDot.Messaging.Specifications
{
    public class TestHttpServer : IHttpServer
    {
        public static TestHttpServer Instance { get; private set; }

        public static void ClearInstance()
        {
            Instance = null;
        }

        readonly IHttpHandler handler;
        bool started;
        
        public string Url { get; private set; }
        
        public TestHttpServer(FixedPortAddress address, IHttpHandler handler)
        {
            Url = address.Url;
            this.handler = handler;

            Instance = this;
        }

        public void Start()
        {
            this.started = true;
        }

        public void StopWork()
        {
            this.started = false;
        }

        public Stream Request(Stream requestStream)
        {
            if (!this.started) return null;

            var outputStream = new MemoryStream();

            this.handler.HandleRequest(requestStream, outputStream);

            return outputStream;
        }
    }
}