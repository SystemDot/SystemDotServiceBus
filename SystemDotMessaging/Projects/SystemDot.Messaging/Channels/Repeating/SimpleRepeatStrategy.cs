using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Repeating
{
    public class SimpleRepeatStrategy : IRepeatStrategy
    {
        readonly IPersistence persistence;
        bool isStarted;

        public SimpleRepeatStrategy(IPersistence persistence)
        {
            this.persistence = persistence;
        }

        public void Repeat(MessageRepeater repeater)
        {
            if(this.isStarted) return;

            this.isStarted = true;
            this.persistence.GetMessages().ForEach(repeater.InputMessage);
        }
    }
}