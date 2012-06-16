using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Http;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Specifications.long_polling
{
    public class TestWebRequestor : IWebRequestor 
    {
        readonly IEnumerable<MessagePayload> messagePayloads;

        public MemoryStream RequestStream { get; private set; }
        
        public MemoryStream ResponseStream { get; private set; }
        
        public string AddressUsed { get; private set; }

        public TestWebRequestor() : this(new List<MessagePayload>()) {}

        public TestWebRequestor(IEnumerable<MessagePayload> messagePayloads)
        {
            this.messagePayloads = messagePayloads;
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

            ResponseStream.Serialise(this.messagePayloads, new BinaryFormatter());
            performOnResponseStream(ResponseStream);
        }
    }
}