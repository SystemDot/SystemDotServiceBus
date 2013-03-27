using System;

namespace SystemDot.Messaging.Batching
{
    class BatchPackager : IMessageProcessor<object, object>
    {
        BatchMessage currentPackage;

        public void InputMessage(object toInput)
        {
            if (!Batch.HasAggregateStarted())
            {
                MessageProcessed(toInput);
                return;
            }

            if(this.currentPackage == null)
            {
                this.currentPackage = new BatchMessage();
                Batch.GetCurrent().Finished += completed =>
                {
                    if (completed) MessageProcessed(this.currentPackage);
                    this.currentPackage = null;
                };
            }

            this.currentPackage.Messages.Add(toInput);
        }

        public event Action<object> MessageProcessed;
    }
}