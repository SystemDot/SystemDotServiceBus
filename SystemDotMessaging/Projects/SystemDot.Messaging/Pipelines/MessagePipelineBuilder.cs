using SystemDot.Core;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging.Pipelines
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
            IocContainerLocator.Locate().Resolve<IMessageBus>().MessageSent += o => processor.InputMessage(o.As<T>());
            return new ProcessorBuilder<T>(processor);
        }

        public ProcessorBuilder<T> WithBusReplyTo<T>(IMessageProcessor<T, T> processor)
        {
            IocContainerLocator.Locate().Resolve<IMessageBus>().MessageReplied += o => processor.InputMessage(o.As<T>());
            return new ProcessorBuilder<T>(processor);
        }

        public ProcessorBuilder<T> WithBusPublishTo<T>(IMessageProcessor<T, T> processor)
        {
            IocContainerLocator.Locate().Resolve<IMessageBus>().MessagePublished += o => processor.InputMessage(o.As<T>());
            return new ProcessorBuilder<T>(processor);
        }

        public ProcessorBuilder<T> WithBusSendDirectTo<T>(IMessageProcessor<T, T> processor)
        {
            IocContainerLocator.Locate().Resolve<IMessageBus>().MessageSentDirect += o => processor.InputMessage(o.As<T>());
            return new ProcessorBuilder<T>(processor);
        }
    }
}