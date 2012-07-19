using System.Collections.Generic;
using SystemDot.Logging;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class SubscriptionRequestHandler : IMessageInputter<MessagePayload>
    {
        readonly IChannelBuilder builder;
        List<EndpointAddress> registry;

        public SubscriptionRequestHandler(IChannelBuilder builder)
        {
            this.builder = builder;
            this.registry = new List<EndpointAddress>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            if (!toInput.IsSubscriptionRequest()) return;

            EndpointAddress subscriberAddress = toInput.GetSubscriptionRequestSchema().SubscriberAddress;

            Logger.Info("Handling subscription request for {0}",
                subscriberAddress);

            if (this.registry.Contains(subscriberAddress))
                return;
            
            registry.Add(subscriberAddress);

            this.builder.Build(toInput.GetSubscriptionRequestSchema());
        }
    }
}