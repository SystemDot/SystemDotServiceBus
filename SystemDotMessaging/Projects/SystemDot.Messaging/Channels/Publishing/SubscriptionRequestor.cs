using System;
using SystemDot.Logging;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscriptionRequestor : ISubscriptionRequestor
    {
        readonly EndpointAddress subscriberAddress;
        readonly bool isDurable;

        public event Action<MessagePayload> MessageProcessed;
        
        public SubscriptionRequestor(EndpointAddress subscriberAddress, bool isDurable)
        {
            this.subscriberAddress = subscriberAddress;
            this.isDurable = isDurable;
        }

        public void Start()
        {
            Logger.Info("Sending subscription request");

            var request = new MessagePayload();
            request.SetSubscriptionRequest(new SubscriptionSchema
            {
                SubscriberAddress = subscriberAddress,
                IsDurable = this.isDurable
            });
            MessageProcessed(request);
        }
    }
}