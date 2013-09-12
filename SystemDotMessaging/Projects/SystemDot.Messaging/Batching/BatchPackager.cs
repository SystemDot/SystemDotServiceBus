using System;
using SystemDot.Logging;

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

            if(currentPackage == null)
            {
                Logger.Debug("Batching {0} message", toInput.GetType().Name);

                currentPackage = new BatchMessage();
                Batch.GetCurrent().Finished += completed =>
                {
                    if (completed) MessageProcessed(currentPackage);
                    currentPackage = null;
                };
            }

            currentPackage.Messages.Add(toInput);
        }

        public event Action<object> MessageProcessed;
    }
}