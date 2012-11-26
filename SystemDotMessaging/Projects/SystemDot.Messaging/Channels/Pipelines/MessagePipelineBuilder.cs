using SystemDot.Ioc;

namespace SystemDot.Messaging.Channels.Pipelines
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
            IocContainerLocator.Locate().Resolve<IBus>().MessageSent += o => processor.InputMessage(o.As<T>());
            return new ProcessorBuilder<T>(processor);
        }

        public void WithBusSendLocalTo<T>(IMessageInputter<T> processor)
        {
            IocContainerLocator.Locate().Resolve<IBus>().MessageSentLocal += o => processor.InputMessage(o.As<T>());
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
    }
}