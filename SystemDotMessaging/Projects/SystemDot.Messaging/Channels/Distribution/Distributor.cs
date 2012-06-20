using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Channels.Distribution
{
    public class Distributor : IChannelEndPoint<MessagePayload>
    {
        readonly MessagePayloadCopier messagePayloadCopier;
        readonly List<DistributionSubscriber> subscribers;
        
        public Distributor(MessagePayloadCopier messagePayloadCopier)
        {
            Contract.Requires(messagePayloadCopier != null);

            this.messagePayloadCopier = messagePayloadCopier;
            this.subscribers = new List<DistributionSubscriber>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            Contract.Requires(toInput != null);
            this.subscribers.ForEach(s => s.Update(this.messagePayloadCopier.Copy(toInput)));        
        }

        public void Subscribe(DistributionSubscriber toSubscribe)
        {
            Contract.Requires(toSubscribe != null);
            this.subscribers.Add(toSubscribe);
        }
    }
}