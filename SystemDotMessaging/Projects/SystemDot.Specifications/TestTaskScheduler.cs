using System;
using SystemDot.Parallelism;

namespace SystemDot.Specifications
{
    public class TestTaskScheduler : ITaskScheduler
    {
        readonly int schedulesPermitted;
        readonly TestCurrentDateProvider currentDateProvider;
        int schedulesExecuted;

        public TestTaskScheduler(int schedulesPermitted, TestCurrentDateProvider currentDateProvider)
        {
            this.schedulesPermitted = schedulesPermitted;
            this.currentDateProvider = currentDateProvider;
        }

        public TimeSpan LastDelay { get; private set;  }

        public void ScheduleTask(TimeSpan delay, Action task)
        {
            if (++schedulesExecuted > this.schedulesPermitted)
                return;

            this.LastDelay = delay;
            this.currentDateProvider.AddToCurrentDate(delay);
            task.Invoke();

        }
    }
}