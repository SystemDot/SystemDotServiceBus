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
        public void Build(EndpointAddress replyAddress)
        {
            MessagePipelineBuilder.Build()
                .With(IocContainer.Resolve<IMessageReciever>())
                .Pump()
                .ToConverter(IocContainer.Resolve<MessagePayloadUnpackager>())
                .ToProcessor(IocContainer.Resolve<ReplyChannelSelector, EndpointAddress>(replyAddress))
                .ToEndPoint(IocContainer.Resolve<MessageHandlerRouter>());
        }
    }
}