using System;
using SystemDot.Logging;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscriptionRequestor : ISubscriptionRequestor
    {
        readonly EndpointAddress subscriberAddress;
        readonly bool isPersistent;

        public event Action<MessagePayload> MessageProcessed;
        
        public SubscriptionRequestor(EndpointAddress subscriberAddress, bool isPersistent)
        {
            this.subscriberAddress = subscriberAddress;
            this.isPersistent = isPersistent;
        }

        public void Start()
        {
            Logger.Info("Sending subscription request");

            var request = new MessagePayload();
            request.SetSubscriptionRequest(new SubscriptionSchema
            {
                SubscriberAddress = subscriberAddress,
                IsPersistent = this.isPersistent
            });
            MessageProcessed(request);
        }
    }
}