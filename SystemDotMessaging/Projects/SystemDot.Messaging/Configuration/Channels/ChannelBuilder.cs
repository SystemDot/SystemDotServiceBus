using SystemDot.Messaging.Channels;

namespace SystemDot.Messaging.Configuration.Channels
{
    public class ChannelBuilder
    {
        public static StartPointBuilder<T> StartsWith<T>(IChannelStartPoint<T> startPoint)
        {
            return new StartPointBuilder<T>(startPoint);
        }
    }
}