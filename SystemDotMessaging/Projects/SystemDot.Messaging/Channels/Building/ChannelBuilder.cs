namespace SystemDot.Messaging.Channels.Building
{
    public class ChannelBuilder
    {
        public static StartPointBuilder<T> StartsWith<T>(IChannelStartPoint<T> startPoint)
        {
            return new StartPointBuilder<T>(startPoint);
        }
    }
}