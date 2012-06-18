using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Channels.Local.Publishing
{
    public class PublisherDistributor : IChannelEndPoint<MessagePayload>
    {
        readonly MessagePayloadCopier messagePayloadCopier;
        readonly List<Subscriber> subscribers;
        
        public PublisherDistributor(MessagePayloadCopier messagePayloadCopier)
        {
            Contract.Requires(messagePayloadCopier != null);

            this.messagePayloadCopier = messagePayloadCopier;
            this.subscribers = new List<Subscriber>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            Contract.Requires(toInput != null);
            this.subscribers.ForEach(s => s.Update(this.messagePayloadCopier.Copy(toInput)));        
        }

        public void Subscribe(Subscriber toSubscribe)
        {
            Contract.Requires(toSubscribe != null);
            this.subscribers.Add(toSubscribe);
        }
    }
}