using System;
using SystemDot.Parallelism;

namespace SystemDot.Specifications
{
    public class TestTaskScheduler : ITaskScheduler
    {
        readonly int schedulesPermitted;
        readonly TestSystemTime systemTime;
        int schedulesExecuted;

        public TestTaskScheduler(int schedulesPermitted, TestSystemTime systemTime)
        {
            this.schedulesPermitted = schedulesPermitted;
            this.systemTime = systemTime;
        }

        public TimeSpan LastDelay { get; private set;  }

        public void ScheduleTask(TimeSpan delay, Action task)
        {
            if (++schedulesExecuted > schedulesPermitted)
                return;

            LastDelay = delay;
            //systemTime.AddToCurrentDate(delay);
            task.Invoke();

        }
    }
}