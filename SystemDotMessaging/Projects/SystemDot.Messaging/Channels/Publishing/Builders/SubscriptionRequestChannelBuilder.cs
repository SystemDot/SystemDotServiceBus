using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriptionRequestChannelBuilder : ISubscriptionRequestChannelBuilder
    {
        public ISubscriptionRequestor Build(EndpointAddress subscriberAddress, EndpointAddress publisherAddress)
        {
            SubscriptionRequestor requestor = IocContainer.Resolve<SubscriptionRequestor, EndpointAddress>(subscriberAddress);

            MessagePipelineBuilder.Build()
                .With(requestor)
                .ToProcessor(IocContainer.Resolve<MessageAddresser, EndpointAddress>(publisherAddress))
                .ToProcessor(IocContainer.Resolve<MessageRepeater, TimeSpan>(new TimeSpan(0, 0, 1)))
                .ToEndPoint(IocContainer.Resolve<IMessageSender>());

            return requestor;
        }
    }
}