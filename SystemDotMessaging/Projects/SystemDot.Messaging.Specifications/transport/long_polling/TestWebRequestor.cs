using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SystemDot.Http;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Specifications.transport.long_polling
{
    public class TestWebRequestor : IWebRequestor
    {
        readonly List<MessagePayload> messages;
        readonly ISerialiser formatter;
        readonly FixedPortAddress toCheck;

        public TestWebRequestor(ISerialiser formatter, FixedPortAddress toCheck)
        {
            this.formatter = formatter;
            this.toCheck = toCheck;
            messages = new List<MessagePayload>();
        }

        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest)
        {
            if (toCheck.Url != address.Url)
                return;

            var request = new MemoryStream();
            toPerformOnRequest(request);
        }

        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest, Action<Stream> toPerformOnResponse)
        {
            if (toCheck.Url != address.Url)
                return;

            var request = new MemoryStream();
            toPerformOnRequest(request);

            var requestMessagePayload = request.Deserialise<MessagePayload>(formatter);
            
            if(!requestMessagePayload.IsLongPollRequest()) 
                return;

            var response = new MemoryStream();

            response.Serialise(
                messages
                    .Where(m => m.GetToAddress() == requestMessagePayload.GetLongPollRequestAddress())
                    .ToList(), 
                formatter);

            toPerformOnResponse(response);
        }

        public void AddMessages(params MessagePayload[] messagePayloads)
        {
            this.messages.AddRange(messagePayloads);
        }
    }
}