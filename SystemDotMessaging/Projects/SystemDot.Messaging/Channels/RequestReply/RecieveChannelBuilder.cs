using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Consuming;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class RecieveChannelBuilder : IRecieveChannelBuilder
    {
        public void Build()
        {
            MessagePipelineBuilder.Build()
                .With(IocContainer.Resolve<IMessageReciever>())
                .Pump()
                .ToConverter(IocContainer.Resolve<MessagePayloadUnpackager>())
                .ToEndPoint(IocContainer.Resolve<MessageHandlerRouter>());
        }

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