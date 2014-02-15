using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Handling.Actions;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing.Builders;
using SystemDot.Messaging.Simple;

namespace SystemDot.Messaging.Publishing
{
    class SubscriptionRequestor : IMessageProcessor<MessagePayload>
    {
        readonly SubscriptionRequestChannelSchema schema;
        ActionSubscriptionToken<MessagingInitialised> token;

        public event Action<MessagePayload> MessageProcessed;
        
        public SubscriptionRequestor(SubscriptionRequestChannelSchema schema)
        {
            Contract.Requires(schema != null);
            this.schema = schema;
            token = Messenger.RegisterHandler<MessagingInitialised>(_ => Start());
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