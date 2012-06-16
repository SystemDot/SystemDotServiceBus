using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization;
using SystemDot.Http;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Pipes;
using SystemDot.Threading;
using SystemDot.Messaging.MessageTransportation.Headers;

namespace SystemDot.Messaging.Recieving
{
    public class LongPollReciever : IWorker
    {
        readonly Address address;
        readonly IPipe<MessagePayload> pipe;
        readonly IWebRequestor requestor;
        readonly IFormatter formatter;

        public LongPollReciever(
            Address address, 
            IPipe<MessagePayload> pipe, 
            IWebRequestor requestor, 
            IFormatter formatter)
        {
            Contract.Requires(address != Address.Empty);
            Contract.Requires(pipe != null);
            Contract.Requires(requestor != null);
            Contract.Requires(formatter != null);

            this.address = address;
            this.pipe = pipe;
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
            var payload = new MessagePayload(this.address);
            payload.SetLongPollRequest();

            return payload;
        }

        void RecieveResponse(Stream responseStream)
        {
            var messages = this.formatter.Deserialize(responseStream).As<IEnumerable<MessagePayload>>();
            foreach (var message in messages)
            {
                this.pipe.Push(message);
            }
        }

        public void StopWork()
        {
        }
    }
}