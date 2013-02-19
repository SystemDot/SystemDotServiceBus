using System;

namespace SystemDot.Messaging.Aggregation
{
    public class AggregateUnpackager : IMessageProcessor<object, object>
    {
        public void InputMessage(object toInput)
        {
            if(toInput is AggregateMessage)
                using (new Aggregate()) 
                    toInput.As<AggregateMessage>().Messages.ForEach(m => MessageProcessed(m));
                
            else
                MessageProcessed(toInput);
        }

        public event Action<object> MessageProcessed;
    }
}