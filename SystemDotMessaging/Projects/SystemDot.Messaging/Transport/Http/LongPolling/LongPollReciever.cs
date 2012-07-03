using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization;
using SystemDot.Http;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Transport.Http.LongPolling
{
    public class LongPollReciever : IWorker, IMessageReciever
    {
        readonly List<EndpointAddress> addresses;
        readonly IWebRequestor requestor;
        readonly IFormatter formatter;

        public event Action<MessagePayload> MessageProcessed;

        public LongPollReciever(IWebRequestor requestor, IFormatter formatter)
        {
            Contract.Requires(requestor != null);
            Contract.Requires(formatter != null);

            this.requestor = requestor;
            this.formatter = formatter;
            this.addresses = new List<EndpointAddress>();
        }

        public void RegisterListeningAddress(EndpointAddress toRegister)
        {
            Contract.Requires(toRegister != EndpointAddress.Empty);
            this.addresses.Add(toRegister);
        }

        public void StartWork()
        {
        }

        public void PerformWork()
        {
            this.addresses.ForEach(SendPut);
        }

        void SendPut(EndpointAddress address)
        {
            this.requestor.SendPut(
                address.GetUrl(), 
                s => this.formatter.Serialize(s, CreateLongPollPayload(address)), 
                RecieveResponse);
        }

        MessagePayload CreateLongPollPayload(EndpointAddress address)
        {
            var payload = new MessagePayload();
            payload.SetToAddress(address);
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