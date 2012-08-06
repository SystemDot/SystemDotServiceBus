using System.Collections.Generic;
using SystemDot.Logging;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class SubscriptionRequestHandler : IMessageInputter<MessagePayload>
    {
        readonly ISendChannelBuilder sendChannelBuilder;
        readonly IRecieveChannelBuilder recieveChannelBuilder;
        readonly List<EndpointAddress> registry;

        public SubscriptionRequestHandler(IRecieveChannelBuilder recieveChannelBuilder, ISendChannelBuilder sendChannelBuilder)
        {
            this.recieveChannelBuilder = recieveChannelBuilder;
            this.sendChannelBuilder = sendChannelBuilder;
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

            this.sendChannelBuilder.Build(toInput.GetSubscriptionRequestSchema().SubscriberAddress);
            this.recieveChannelBuilder.Build();
        }
    }
}