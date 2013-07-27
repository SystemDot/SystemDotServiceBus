using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemDot.Http;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.Remote.Clients;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Specifications
{
    public class TestWebRequestor : IWebRequestor
    {
        readonly List<MessagePayload> messages;
        readonly ISerialiser formatter;
        
        public List<Stream> RequestsMade { get; private set; }
        
        public TestWebRequestor(ISerialiser formatter)
        {
            this.formatter = formatter;
            this.messages = new List<MessagePayload>();

            RequestsMade = new List<Stream>();
        }

        public T DeserialiseSingleRequest<T>()
        {
            return RequestsMade.Single().Deserialise<T>(formatter);
        }

        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest)
        {
            var request = new MemoryStream();
            toPerformOnRequest(request);
            
            RequestsMade.Add(request);
        }

        public void SendPut(
            FixedPortAddress address, 
            Action<Stream> toPerformOnRequest, 
            Action<Stream> toPerformOnResponse, 
            Action<Exception> toPerformOnError, 
            Action toPerformOnCompletion)
        {
            var request = new MemoryStream();
            toPerformOnRequest(request);
            
            RequestsMade.Add(request);

            var requestMessagePayload = request.Deserialise<MessagePayload>(formatter);
            
            if(!requestMessagePayload.IsLongPollRequest()) return;

            var response = new MemoryStream();

            List<MessagePayload> matching = 
                messages.Where(m => m.GetToAddress().Server == requestMessagePayload.GetLongPollRequestServerPath()).ToList();

            response.Serialise(matching, formatter);

            matching.ForEach(m => messages.Remove(m));

            toPerformOnResponse(response);
            toPerformOnCompletion();
        }

        public void AddMessages(params MessagePayload[] messagePayloads)
        {
            messages.AddRange(messagePayloads);
        }
    }
}