using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Configuration.Repeating
{
    public interface IRepeatMessagesConfigurer
    {
        void SetMessageRepeatingStrategy(IRepeatStrategy strategy);
    }
}