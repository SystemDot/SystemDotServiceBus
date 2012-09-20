using System;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Storage;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Messages.Processing.Repeating
{
    public class DurableMessageRepeater : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly IMessageCache cache;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskScheduler scheduler;

        public DurableMessageRepeater(
            IMessageCache cache, 
            ICurrentDateProvider currentDateProvider, 
            ITaskScheduler scheduler)
        {
            Contract.Requires(cache != null);
            
            this.cache = cache;
            this.currentDateProvider = currentDateProvider;
            this.scheduler = scheduler;
        }

        public void InputMessage(MessagePayload toInput)
        {
            toInput.SetLastTimeSent(this.currentDateProvider.Get());
            toInput.IncreaseAmountSent();
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;

        public void Start()
        {
            this.cache.GetAll().ForEach(m =>
            {
                if (m.GetLastTimeSent() <= this.currentDateProvider.Get().AddSeconds(-GetDelay(m))) 
                    InputMessage(m);
            });

            this.scheduler.ScheduleTask(new TimeSpan(0, 0, 0, 1), Start);
        }

        static int GetDelay(MessagePayload toGetDelayFor)
        {
            return toGetDelayFor.GetAmountSent() < 3 ? toGetDelayFor.GetAmountSent() : 4;
        }
    }
}