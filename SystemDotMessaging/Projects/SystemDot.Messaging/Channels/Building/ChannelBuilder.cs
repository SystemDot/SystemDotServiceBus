using SystemDot.Messaging.Channels.Messages;

namespace SystemDot.Messaging.Channels.Building
{
    public class ChannelBuilder
    {
        public static ChannelBuilder Build()
        {
            return new ChannelBuilder();
        }

        public ProcessorBuilder<T, T> With<T>(IMessageProcessor<T> startPoint)
        {
            return new ProcessorBuilder<T, T>(startPoint);
        }
    }
}