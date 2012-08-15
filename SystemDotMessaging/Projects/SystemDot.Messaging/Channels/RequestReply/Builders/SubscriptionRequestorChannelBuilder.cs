using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class SubscriptionRequestorChannelBuilder : ISubscriptionRequestorChannelBuilder
    {
        public ISubscriptionRequestor Build(EndpointAddress address, EndpointAddress recieverAddress)
        {
            var requestor = IocContainer.Resolve<ISubscriptionRequestor, EndpointAddress>(address);

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