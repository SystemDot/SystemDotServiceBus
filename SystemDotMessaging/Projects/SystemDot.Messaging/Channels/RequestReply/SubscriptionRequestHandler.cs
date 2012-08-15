using System;
using System.Collections.Generic;
using SystemDot.Logging;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class SubscriptionRequestHandler : IMessageInputter<MessagePayload>
    {
        readonly IReplyChannelBuilder replyChannelBuilder;
        readonly IRequestRecieveChannelBuilder requestRecieveChannelBuilder;
        readonly List<EndpointAddress> registry;

        public SubscriptionRequestHandler(IRequestRecieveChannelBuilder requestRecieveChannelBuilder, IReplyChannelBuilder replyChannelBuilder)
        {
            this.requestRecieveChannelBuilder = requestRecieveChannelBuilder;
            this.replyChannelBuilder = replyChannelBuilder;
            this.registry = new List<EndpointAddress>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            if (!toInput.IsSubscriptionRequest()) return;

            EndpointAddress subscriberAddress = toInput.GetSubscriptionRequestSchema().SubscriberAddress;

            Logger.Info("Handling subscription request for {0}", subscriberAddress);

            if (this.registry.Contains(subscriberAddress))
                return;
            
            registry.Add(subscriberAddress);

            Guid channelIdentifier = this.requestRecieveChannelBuilder.Build();
            this.replyChannelBuilder.Build(channelIdentifier, toInput.GetSubscriptionRequestSchema().SubscriberAddress);
        }
    }
}