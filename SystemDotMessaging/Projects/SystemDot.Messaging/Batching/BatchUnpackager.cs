using System;

namespace SystemDot.Messaging.Batching
{
    public class BatchUnpackager : IMessageProcessor<object, object>
    {
        public void InputMessage(object toInput)
        {
            if(toInput is BatchMessage)
                ProcessMessagesInBatch(toInput);
            else
                MessageProcessed(toInput);
        }

        void ProcessMessagesInBatch(object toInput)
        {
            using (var batch = new Batch())
            {
                toInput.As<BatchMessage>().Messages.ForEach(m => this.MessageProcessed(m));
                batch.Complete();
            }
        }

        public event Action<object> MessageProcessed;
    }
}