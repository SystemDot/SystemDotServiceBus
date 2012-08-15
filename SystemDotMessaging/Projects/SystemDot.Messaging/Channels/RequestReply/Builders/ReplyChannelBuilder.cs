using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplyChannelBuilder : IReplyChannelBuilder
    {
        public void Build(Guid channelIdentifier, EndpointAddress recieverAddress)
        {
            MessagePipelineBuilder.Build()
                .WithBusReplyTo(IocContainer.Resolve<MessageReplyFilter, Guid>(channelIdentifier))
                .Pipe()
                .ToConverter(IocContainer.Resolve<MessagePayloadPackager>())
                .ToProcessor(IocContainer.Resolve<MessageAddresser, EndpointAddress>(recieverAddress))
                .ToEndPoint(IocContainer.Resolve<IMessageSender>());
        }
    }
}