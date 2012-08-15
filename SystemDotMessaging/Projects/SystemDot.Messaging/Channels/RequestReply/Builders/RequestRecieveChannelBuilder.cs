using System;
using SystemDot.Messaging.Messages.Handling;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestRecieveChannelBuilder : IRequestRecieveChannelBuilder
    {
        public Guid Build()
        {
            Guid channelIdentifier = Guid.NewGuid();
            
            MessagePipelineBuilder.Build()
                .With(IocContainer.Resolve<IMessageReciever>())
                .Pump()
                .ToConverter(IocContainer.Resolve<MessagePayloadUnpackager>())
                .ToProcessor(IocContainer.Resolve<ReplyChannelReserver, Guid>(channelIdentifier))
                .ToEndPoint(IocContainer.Resolve<MessageHandlerRouter>());

            return channelIdentifier;
        }
    }
}