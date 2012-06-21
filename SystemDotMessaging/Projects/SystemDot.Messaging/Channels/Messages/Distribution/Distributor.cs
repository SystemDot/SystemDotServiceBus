using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Channels.Messages.Distribution
{
    public class Distributor : IDistributor
    {
        readonly MessagePayloadCopier messagePayloadCopier;
        readonly List<IDistributionSubscriber> subscribers;
        
        public Distributor(MessagePayloadCopier messagePayloadCopier)
        {
            Contract.Requires(messagePayloadCopier != null);

            this.messagePayloadCopier = messagePayloadCopier;
            this.subscribers = new List<IDistributionSubscriber>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            Contract.Requires(toInput != null);
            this.subscribers.ForEach(s => s.Recieve(this.messagePayloadCopier.Copy(toInput)));        
        }

        public void Subscribe(IDistributionSubscriber toSubscribe)
        {
            Contract.Requires(toSubscribe != null);
            this.subscribers.Add(toSubscribe);
        }
    }
}