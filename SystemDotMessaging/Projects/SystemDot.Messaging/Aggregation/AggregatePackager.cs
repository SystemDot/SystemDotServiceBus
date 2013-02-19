using System;

namespace SystemDot.Messaging.Aggregation
{
    public class AggregatePackager : IMessageProcessor<object, object>
    {
        AggregateMessage currentPackage;

        public void InputMessage(object toInput)
        {
            if (!Aggregate.HasAggregateStarted())
            {
                this.MessageProcessed(toInput);
                return;
            }

            if(this.currentPackage == null)
            {
                this.currentPackage = new AggregateMessage();
                Aggregate.GetCurrent().Finished += () =>
                {
                    MessageProcessed(this.currentPackage);
                    this.currentPackage = null;
                };
            }

            this.currentPackage.Messages.Add(toInput);
        }

        public event Action<object> MessageProcessed;
    }
}