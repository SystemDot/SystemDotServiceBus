using SystemDot.Ioc;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging.Pipelines
{
    public class MessagePipelineBuilder
    {
        public static bool BuildSynchronousPipelines { get; set; }

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
            IocContainerLocator.Locate().Resolve<IBus>().MessageSent += o => processor.InputMessage(o.As<T>());
            return new ProcessorBuilder<T>(processor);
        }

        public ProcessorBuilder<T> WithBusReplyTo<T>(IMessageProcessor<T, T> processor)
        {
            IocContainerLocator.Locate().Resolve<IBus>().MessageReplied += o => processor.InputMessage(o.As<T>());
            return new ProcessorBuilder<T>(processor);
        }

        public ProcessorBuilder<T> WithBusPublishTo<T>(IMessageProcessor<T, T> processor)
        {
            IocContainerLocator.Locate().Resolve<IBus>().MessagePublished += o => processor.InputMessage(o.As<T>());
            return new ProcessorBuilder<T>(processor);
        }

        public ProcessorBuilder<T> WithBusSendDirectTo<T>(IMessageProcessor<T, T> processor)
        {
            IocContainerLocator.Locate().Resolve<IBus>().MessageSentDirect += o => processor.InputMessage(o.As<T>());
            return new ProcessorBuilder<T>(processor);
        }
    }
}