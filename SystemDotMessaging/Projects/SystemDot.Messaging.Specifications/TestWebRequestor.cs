using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemDot.Http;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.Remote;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Specifications
{
    public class TestWebRequestor : IWebRequestor
    {
        readonly List<MessagePayload> messages;
        readonly ISerialiser formatter;
        readonly FixedPortAddress toCheck;

        public Stream LastPutRequestSent { get; set; }

        public TestWebRequestor(ISerialiser formatter, FixedPortAddress toCheck)
        {
            this.formatter = formatter;
            this.toCheck = toCheck;
            this.messages = new List<MessagePayload>();
        }

        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest)
        {
            if (this.toCheck.Url != address.Url)
                return;

            var request = new MemoryStream();
            toPerformOnRequest(request);

            LastPutRequestSent = request;
        }

        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest, Action<Stream> toPerformOnResponse)
        {
            if (this.toCheck.Url != address.Url)
                return;

            var request = new MemoryStream();
            toPerformOnRequest(request);

            LastPutRequestSent = request;

            var requestMessagePayload = request.Deserialise<MessagePayload>(this.formatter);
            
            if(!requestMessagePayload.IsLongPollRequest()) 
                return;

            var response = new MemoryStream();

            response.Serialise(
                this.messages
                    .Where(m => m.GetToAddress() == requestMessagePayload.GetLongPollRequestAddress())
                    .ToList(), 
                this.formatter);

            toPerformOnResponse(response);
        }

        public void AddMessages(params MessagePayload[] messagePayloads)
        {
            this.messages.AddRange(messagePayloads);
        }
    }
}