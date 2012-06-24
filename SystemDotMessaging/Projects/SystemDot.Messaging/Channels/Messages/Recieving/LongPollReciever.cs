using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization;
using SystemDot.Http;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Threading;
using SystemDot.Messaging.MessageTransportation.Headers;

namespace SystemDot.Messaging.Channels.Messages.Recieving
{
    public class LongPollReciever : IWorker, IMessageProcessor<MessagePayload>
    {
        readonly Address address;
        readonly IWebRequestor requestor;
        readonly IFormatter formatter;

        public event Action<MessagePayload> MessageProcessed;

        public LongPollReciever(Address address, IWebRequestor requestor, IFormatter formatter)
        {
            Contract.Requires(address != Address.Empty);
            Contract.Requires(requestor != null);
            Contract.Requires(formatter != null);

            this.address = address;
            this.requestor = requestor;
            this.formatter = formatter;
        }

        public void StartWork()
        {
        }

        public void PerformWork()
        {
            this.requestor.SendPut(this.address.Url, SendRequest, RecieveResponse);
        }

        void SendRequest(Stream requestStream)
        {
            this.formatter.Serialize(requestStream, CreateLongPollPayload()); 
        }

        MessagePayload CreateLongPollPayload()
        {
            var payload = new MessagePayload();
            payload.SetToAddress(this.address);
            payload.SetLongPollRequest();

            return payload;
        }

        void RecieveResponse(Stream responseStream)
        {
            var messages = this.formatter.Deserialize(responseStream).As<IEnumerable<MessagePayload>>();
            foreach (var message in messages)
            {
                this.MessageProcessed(message);
            }
        }

        public void StopWork()
        {
        }
    }
}