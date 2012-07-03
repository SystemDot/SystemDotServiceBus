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
    }
}