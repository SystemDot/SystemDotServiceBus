using System;
using SystemDot.Core;
using SystemDot.Core.Collections;

namespace SystemDot.Messaging.Batching
{
    class BatchUnpackager : IMessageProcessor<object, object>
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