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
        Exception exceptionToThrowOnSendPut;

        public List<Stream> RequestsMade { get; private set; }
        
        public TestWebRequestor(ISerialiser formatter)
        {
            this.formatter = formatter;
            messages = new List<MessagePayload>();
            RequestsMade = new List<Stream>();
        }

        public void ThrowExceptionOnPut(Exception toThrow)
        {
            exceptionToThrowOnSendPut = toThrow;
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
            if (exceptionToThrowOnSendPut != null) toPerformOnError(exceptionToThrowOnSendPut);

            var request = new MemoryStream();
            toPerformOnRequest(request);
            
            RequestsMade.Add(request);

            var response = new MemoryStream();

            response.Serialise(messages, formatter);
            messages.ForEach(m => messages.Remove(m));

            toPerformOnResponse(response);
            toPerformOnCompletion();
        }

        public void AddMessages(params MessagePayload[] messagePayloads)
        {
            messages.AddRange(messagePayloads);
        }
    }
}