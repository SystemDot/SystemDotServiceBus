using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Repeating
{
    public interface IRepeatStrategy
    {
        void Repeat(MessageRepeater repeater, MessageCache messageCache, ICurrentDateProvider currentDateProvider);
    }
}