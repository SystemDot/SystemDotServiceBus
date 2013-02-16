using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemDot.Http;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.Remote;
using SystemDot.Messaging.Transport.Http.Remote.Clients;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Specifications
{
    public class TestWebRequestor : IWebRequestor
    {
        readonly List<MessagePayload> messages;
        readonly ISerialiser formatter;
        readonly FixedPortAddress toCheck;

        public List<Stream> RequestsMade { get; private set; }

        public TestWebRequestor(ISerialiser formatter, FixedPortAddress toCheck)
        {
            this.formatter = formatter;
            this.toCheck = toCheck;
            this.messages = new List<MessagePayload>();
            RequestsMade = new List<Stream>();
        }

        public T DeserialiseSingleRequest<T>()
        {
            return RequestsMade.Single().Deserialise<T>(this.formatter);
        }

        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest)
        {
            if (this.toCheck.Url != address.Url)
                return;

            var request = new MemoryStream();
            toPerformOnRequest(request);
            
            RequestsMade.Add(request);
        }

        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest, Action<Stream> toPerformOnResponse)
        {
            if (this.toCheck.Url != address.Url)
                return;

            var request = new MemoryStream();
            toPerformOnRequest(request);
            
            RequestsMade.Add(request);

            var requestMessagePayload = request.Deserialise<MessagePayload>(this.formatter);
            
            if(!requestMessagePayload.IsLongPollRequest()) 
                return;

            var response = new MemoryStream();

            List<MessagePayload> matching = 
                this.messages
                    .Where(m => m.GetToAddress().ServerPath == requestMessagePayload.GetLongPollRequestServerPath()).ToList();

            response.Serialise(matching, this.formatter);

            matching.ForEach(m => this.messages.Remove(m));

            toPerformOnResponse(response);
        }

        public void AddMessages(params MessagePayload[] messagePayloads)
        {
            this.messages.AddRange(messagePayloads);
        }
    }
}