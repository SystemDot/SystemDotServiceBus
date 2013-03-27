using System;

namespace SystemDot.Messaging.Filtering
{
    class MessageFilter : IMessageProcessor<object, object>
    {
        readonly IMessageFilterStrategy filterStrategy;

        public MessageFilter(IMessageFilterStrategy filterStrategy)
        {
            this.filterStrategy = filterStrategy;
        }

        public void InputMessage(object toInput)
        {
            if(this.filterStrategy.PassesThrough(toInput))
                this.MessageProcessed(toInput);
        }

        public event Action<object> MessageProcessed;
    }
}