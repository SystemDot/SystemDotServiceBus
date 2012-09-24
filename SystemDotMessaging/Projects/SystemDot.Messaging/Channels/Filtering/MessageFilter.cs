using System;

namespace SystemDot.Messaging.Channels.Filtering
{
    public class MessageFilter : IMessageProcessor<object, object>
    {
        readonly IMessageFilterStrategy filterStrategy;

        public MessageFilter(IMessageFilterStrategy filterStrategy)
        {
            this.filterStrategy = filterStrategy;
        }

        public void InputMessage(object toInput)
        {
            if(this.filterStrategy.PassesThrough(toInput))
                MessageProcessed(toInput);
        }

        public event Action<object> MessageProcessed;
    }
}