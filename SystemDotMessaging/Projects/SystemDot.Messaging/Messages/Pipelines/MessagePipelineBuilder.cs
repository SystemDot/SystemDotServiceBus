using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Messages.Processing;

namespace SystemDot.Messaging.Messages.Pipelines
{
    public class MessagePipelineBuilder
    {
        public static MessagePipelineBuilder Build()
        {
            return new MessagePipelineBuilder();
        }

        public ProcessorBuilder<T> With<T>(IMessageProcessor<T> startPoint)
        {
            return new ProcessorBuilder<T>(startPoint);
        }

        public ProcessorBuilder<T> WithBusSendTo<T>(IMessageProcessor<T, T> processor)
        {
            IocContainer.Resolve<IBus>().MessageSent += o => processor.InputMessage(o.As<T>());
            return new ProcessorBuilder<T>(processor);
        }

        public ProcessorBuilder<T> WithBusPublishTo<T>(IMessageProcessor<T, T> processor)
        {
            IocContainer.Resolve<IBus>().MessagePublished += o => processor.InputMessage(o.As<T>());
            return new ProcessorBuilder<T>(processor);
        }
    }
}