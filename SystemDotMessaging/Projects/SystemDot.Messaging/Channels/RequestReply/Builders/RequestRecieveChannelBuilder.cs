using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Handling;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestRecieveChannelBuilder : IRequestRecieveChannelBuilder
    {
        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(IocContainer.Resolve<IMessageReciever>())
                .Pump()
                .ToProcessor(IocContainer.Resolve<ReplyChannelSelector>())
                .ToConverter(IocContainer.Resolve<MessagePayloadUnpackager>())
                .ToEndPoint(IocContainer.Resolve<MessageHandlerRouter>());
        }
    }
}