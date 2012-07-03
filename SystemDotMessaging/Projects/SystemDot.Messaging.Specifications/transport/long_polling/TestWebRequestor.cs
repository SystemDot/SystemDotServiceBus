using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
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

        public void SendPut(FixedPortAddress address, Action<Stream> performOnRequestStream)
        {
        }

        public void SendPut(FixedPortAddress address, Action<Stream> performOnRequestStream, Action<Stream> performOnResponseStream)
        {
            var request = new MemoryStream();
            performOnRequestStream(request);

            var requestMessagePayload = request.Deserialise<MessagePayload>(formatter);
            if(!requestMessagePayload.IsLongPollRequest()) return;

            var response = new MemoryStream();

            response.Serialise(messages[address], formatter);
            performOnResponseStream(response);
        }

        public void AddMessages(FixedPortAddress address, params MessagePayload[] messagePayloads)
        {
            messages[address] = messagePayloads;
        }

        
    }
}