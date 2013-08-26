using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Publishing
{
    class SubscriptionRequestor : IMessageProcessor<MessagePayload>
    {
        readonly EndpointAddress subscriberAddress;
        readonly bool isDurable;

        public event Action<MessagePayload> MessageProcessed;
        
        public SubscriptionRequestor(EndpointAddress subscriberAddress, bool isDurable)
        {
            Contract.Requires(subscriberAddress != null);

            this.subscriberAddress = subscriberAddress;
            this.isDurable = isDurable;

            Messenger.Register<MessagingInitialised>(_ => Start());
        }

        void Start()
        {
            Logger.Info("Sending subscription request for {0}", subscriberAddress);

            var request = new MessagePayload();
            request.SetSubscriptionRequest(new SubscriptionSchema
            {
                SubscriberAddress = subscriberAddress,
                IsDurable = isDurable
            });
            MessageProcessed(request);
        }
    }
}