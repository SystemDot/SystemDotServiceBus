using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Messages.Distribution
{
    public class Distributor : IDistributor
    {
        readonly MessagePayloadCopier messagePayloadCopier;
        readonly List<IMessageInputter<MessagePayload>> subscribers;
        
        public Distributor(MessagePayloadCopier messagePayloadCopier)
        {
            Contract.Requires(messagePayloadCopier != null);

            this.messagePayloadCopier = messagePayloadCopier;
            this.subscribers = new List<IMessageInputter<MessagePayload>>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            Contract.Requires(toInput != null);
            this.subscribers.ForEach(s => s.InputMessage(this.messagePayloadCopier.Copy(toInput)));        
        }

        public void Subscribe(IMessageInputter<MessagePayload> toSubscribe)
        {
            Contract.Requires(toSubscribe != null);
            this.subscribers.Add(toSubscribe);
        }
    }
}