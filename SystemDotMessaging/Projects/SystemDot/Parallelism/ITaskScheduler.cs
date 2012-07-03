using System;

namespace SystemDot.Parallelism
{
    public interface ITaskScheduler 
    {
        void ScheduleTask(TimeSpan delay, Action task);
    }
}