using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Repeating
{
    public class SimpleRepeatStrategy : IRepeatStrategy
    {
        bool isStarted;

        public void Repeat(MessageRepeater repeater, MessageCache messageCache, ICurrentDateProvider currentDateProvider)
        {
            if(this.isStarted) return;

            this.isStarted = true;
            messageCache.GetMessages().ForEach(repeater.InputMessage);
        }
    }
}