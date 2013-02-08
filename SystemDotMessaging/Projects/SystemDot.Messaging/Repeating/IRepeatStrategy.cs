using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Repeating
{
    public interface IRepeatStrategy
    {
        void Repeat(MessageRepeater repeater, MessageCache messageCache, ISystemTime systemTime);
    }
}