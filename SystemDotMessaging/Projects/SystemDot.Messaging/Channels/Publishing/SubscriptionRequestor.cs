using System;
using SystemDot.Logging;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscriptionRequestor : ISubscriptionRequestor
    {
        readonly EndpointAddress subscriberAddress;
        
        public event Action<MessagePayload> MessageProcessed;
        
        public SubscriptionRequestor(EndpointAddress subscriberAddress)
        {
            this.subscriberAddress = subscriberAddress;
        }

        public void Start()
        {
            Logger.Info("Sending subscription request");

            var request = new MessagePayload();
            request.SetSubscriptionRequest(new SubscriptionSchema(subscriberAddress));
            MessageProcessed(request);
        }
    }
}