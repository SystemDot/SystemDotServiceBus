using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Handling;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplyRecieveChannelBuilder : IReplyRecieveChannelBuilder
    {
        public void Build(params IMessageProcessor<object, object>[] hooks)
        {
            MessagePipelineBuilder.Build()
                .With(IocContainer.Resolve<IMessageReciever>())
                .Pump()
                .ToConverter(IocContainer.Resolve<MessagePayloadUnpackager>())
                .ToProcessors(hooks)
                .ToEndPoint(IocContainer.Resolve<MessageHandlerRouter>());
        }
    }
}