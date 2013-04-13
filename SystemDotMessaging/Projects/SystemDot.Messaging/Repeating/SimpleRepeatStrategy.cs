using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Repeating
{
    public class SimpleRepeatStrategy : IRepeatStrategy
    {
        bool isStarted;

        public void Repeat(MessageRepeater repeater, IMessageCache messageCache, ISystemTime systemTime)
        {
            if(this.isStarted) return;

            this.isStarted = true;
            messageCache.GetOrderedMessages().ForEach(repeater.InputMessage);
        }
    }
}