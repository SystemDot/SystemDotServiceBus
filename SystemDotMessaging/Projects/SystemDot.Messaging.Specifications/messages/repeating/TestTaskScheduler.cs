using System;
using System.Threading;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Specifications.messages.repeating
{
    public class TestTaskScheduler : ITaskScheduler
    {
        readonly int schedulesPermitted;
        int schedulesExecuted;

        public TestTaskScheduler(int schedulesPermitted)
        {
            this.schedulesPermitted = schedulesPermitted;
        }

        public TimeSpan LastDelay { get; private set;  }

        public void ScheduleTask(TimeSpan delay, Action task)
        {
            if (++schedulesExecuted > this.schedulesPermitted)
                return;

            this.LastDelay = delay;
            task.Invoke();
        }
    }
}