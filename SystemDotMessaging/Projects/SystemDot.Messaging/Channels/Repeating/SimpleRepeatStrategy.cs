using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Repeating
{
    public class SimpleRepeatStrategy : IRepeatStrategy
    {
        readonly MessageCache messageCache;
        bool isStarted;

        public SimpleRepeatStrategy(MessageCache messageCache)
        {
            this.messageCache = messageCache;
        }

        public void Repeat(MessageRepeater repeater)
        {
            if(this.isStarted) return;

            this.isStarted = true;
            this.messageCache.GetMessages().ForEach(repeater.InputMessage);
        }
    }
}