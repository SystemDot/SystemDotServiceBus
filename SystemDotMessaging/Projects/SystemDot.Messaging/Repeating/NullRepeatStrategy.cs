using SystemDot.Core;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Repeating
{
    public class NullRepeatStrategy : LoggingRepeatStrategy, IRepeatStrategy
    {
        public void Repeat(MessageRepeater repeater, IMessageCache messageCache, ISystemTime systemTime)
        {
        }
    }
}