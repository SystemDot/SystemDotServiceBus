using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class SubscriptionHandlerChannelBuilder : ISubscriptionHandlerChannelBuilder
    {
        public SubscriptionRequestor Build(EndpointAddress address, EndpointAddress recieverAddress)
        {
            var requestor = IocContainer.Resolve<SubscriptionRequestor, EndpointAddress>(address);

            MessagePipelineBuilder.Build()
                .With(requestor)
                .Pump()
                .ToProcessor(IocContainer.Resolve<MessageAddresser, EndpointAddress>(recieverAddress))
                .ToProcessor(IocContainer.Resolve<MessageRepeater, TimeSpan>(new TimeSpan(0, 0, 1)))
                .ToEndPoint(IocContainer.Resolve<IMessageSender>());

            return requestor;
        }
    }
}