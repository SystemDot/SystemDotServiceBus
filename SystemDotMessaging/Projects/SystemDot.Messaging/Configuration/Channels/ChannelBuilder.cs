namespace SystemDot.Messaging.Configuration.Channels
{
    public class ChannelBuilder
    {
        public static StartPointBuilder<T> StartsWith<T>(IMessageStartPoint<T> startPoint)
        {
            return new StartPointBuilder<T>(startPoint);
        }
    }
}