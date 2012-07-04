using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Messages.Processing
{
    public class MessageRepeater : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly TimeSpan delay;
        readonly ITaskScheduler taskScheduler;

        public MessageRepeater(TimeSpan delay, ITaskScheduler taskScheduler)
        {
            Contract.Requires(delay != TimeSpan.MinValue);
            Contract.Requires(taskScheduler != null);


            this.delay = delay;
            this.taskScheduler = taskScheduler;
        }

        public void InputMessage(MessagePayload toInput)
        {
            MessageProcessed(toInput);
            ScheduleNextSend(toInput, this.delay);
        }

        void ScheduleNextSend(MessagePayload toInput, TimeSpan nextDelay)
        {
            taskScheduler.ScheduleTask(
                nextDelay, 
                () =>
                {
                    MessageProcessed(toInput);
                    ScheduleNextSend(toInput, new TimeSpan(nextDelay.Ticks * 2));
                });
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}