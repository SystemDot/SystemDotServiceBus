namespace SystemDot.Messaging.Channels.Repeating
{
    public interface IRepeatStrategy
    {
        void Repeat(MessageRepeater repeater);
    }
}