using System;
using SystemDot.Parallelism;

namespace SystemDot.Specifications.parallelism
{
    public class ZeroTimespanPassThroughTaskScheduler : ITaskScheduler
    {
        public void ScheduleTask(TimeSpan delay, Action task)
        {
            if (delay == TimeSpan.FromMilliseconds(1)) task();
        }
    }
}