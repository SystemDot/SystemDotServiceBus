using System;
using System.IO;
using SystemDot.Http;

namespace SystemDot.Messaging.Specifications.long_polling
{
    public class TestWebRequestor : IWebRequestor 
    {
        public MemoryStream RequestStream { get; private set; }
        
        public MemoryStream ResponseStream { get; private set; }
        
        public string AddressUsed { get; private set; }

        public TestWebRequestor()
        {
            RequestStream = new MemoryStream();
            ResponseStream = new MemoryStream();
        }

        public void SendPut(string address, Action<Stream> performOnRequestStream)
        {
            this.AddressUsed = address;
            performOnRequestStream(RequestStream);
        }

        public void SendPut(string address, Action<Stream> performOnRequestStream, Action<Stream> performOnResponseStream)
        {
            SendPut(address, performOnRequestStream);
            performOnResponseStream(ResponseStream);
        }
    }
}