using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing.Builders;

namespace SystemDot.Messaging.Publishing
{
    class SubscriptionRequestor : IMessageProcessor<MessagePayload>
    {
        readonly SubscriptionRequestChannelSchema schema;
        
        public event Action<MessagePayload> MessageProcessed;
        
        public SubscriptionRequestor(SubscriptionRequestChannelSchema schema)
        {
            Contract.Requires(schema != null);
            this.schema = schema;
            Messenger.Register<MessagingInitialised>(_ => Start());
        }

        void Start()
        {
            Logger.Info("Sending subscription request for {0}", schema.SubscriberAddress);

            var request = new MessagePayload();
            request.SetSubscriptionRequest(new SubscriptionSchema
            {
                SubscriberAddress = schema.SubscriberAddress,
                IsDurable = schema.IsDurable,
                RepeatStrategy = schema.RepeatStrategy
            });
            MessageProcessed(request);
        }
    }
}