using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;
using SystemDot.Http;
using SystemDot.Logging;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.Http.LongPolling
{
    public class LongPollReciever : IMessageReciever
    {
        readonly IWebRequestor requestor;
        readonly ISerialiser formatter;
        readonly ITaskLooper looper;

        public event Action<MessagePayload> MessageProcessed;

        public LongPollReciever(IWebRequestor requestor, ISerialiser formatter, ITaskLooper looper)
        {
            Contract.Requires(requestor != null);
            Contract.Requires(formatter != null);
            Contract.Requires(looper != null);
            
            this.requestor = requestor;
            this.formatter = formatter;
            this.looper = looper;
        }

        public void RegisterListeningAddress(EndpointAddress toRegister)
        {
            Contract.Requires(toRegister != EndpointAddress.Empty);
            
            this.looper.RegisterToLoop(() => Poll(toRegister));
        }

        Task Poll(EndpointAddress address)
        {
            Logger.Info("Long polling for messages for {0}", address);

            return this.requestor.SendPut(
                address.GetUrl(), 
                s => this.formatter.Serialise(s, CreateLongPollPayload(address)), 
                RecieveResponse);
        }

        MessagePayload CreateLongPollPayload(EndpointAddress address)
        {
            var payload = new MessagePayload();
            payload.SetLongPollRequest(address);

            return payload;
        }

        void RecieveResponse(Stream responseStream)
        {
            var messages = this.formatter.Deserialise(responseStream).As<IEnumerable<MessagePayload>>();

            foreach (var message in messages)
            {
                Logger.Info("Recieved message");
                this.MessageProcessed(message);
            }
        }
    }
}