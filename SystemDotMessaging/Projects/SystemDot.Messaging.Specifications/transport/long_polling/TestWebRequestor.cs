using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SystemDot.Http;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Messaging.Messages.Packaging.Headers;
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

        public Task SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest)
        {
            return new Task(() => { });
        }

        public Task SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest, Action<Stream> toPerformOnResponse)
        {
            if (toCheck.Url != address.Url)
                return new Task(() => { });

            var request = new MemoryStream();
            toPerformOnRequest(request);

            var requestMessagePayload = request.Deserialise<MessagePayload>(formatter);
            
            if(!requestMessagePayload.IsLongPollRequest()) 
                return new Task(() => { });

            var response = new MemoryStream();

            response.Serialise(
                messages
                    .Where(m => m.GetToAddress() == requestMessagePayload.GetLongPollRequestAddress())
                    .ToList(), 
                formatter);

            toPerformOnResponse(response);

            return new Task(() => { });
        }

        public void AddMessages(params MessagePayload[] messagePayloads)
        {
            this.messages.AddRange(messagePayloads);
        }
    }
}