using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SystemDot.Http;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Transport.Http.LongPolling;

namespace SystemDot.Messaging.Specifications.transport.long_polling
{
    public class TestWebRequestor : IWebRequestor
    {
        readonly Dictionary<FixedPortAddress, MessagePayload[]> messages;
        readonly IFormatter formatter;

        public TestWebRequestor(IFormatter formatter)
        {
            this.formatter = formatter;
            messages = new Dictionary<FixedPortAddress, MessagePayload[]>();
        }

        public Task SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest)
        {
            return new Task(() => { });
        }

        public Task SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest, Action<Stream> toPerformOnResponse)
        {
            var request = new MemoryStream();
            toPerformOnRequest(request);

            var requestMessagePayload = request.Deserialise<MessagePayload>(formatter);
            if(!requestMessagePayload.IsLongPollRequest()) return new Task(() => { });

            var response = new MemoryStream();

            response.Serialise(messages[address], formatter);
            toPerformOnResponse(response);

            return new Task(() => { });
        }

        public void AddMessages(FixedPortAddress address, params MessagePayload[] messagePayloads)
        {
            messages[address] = messagePayloads;
        }
    }
}