using System;
using System.Timers;

namespace SystemDot.Parallelism
{
    public class TaskScheduler : ITaskScheduler 
    {
        public void ScheduleTask(TimeSpan delay, Action task)
        {
            var timer = new Timer(delay.TotalMilliseconds);
            timer.Elapsed += (sender, args) => task();
            timer.AutoReset = false;
            timer.Enabled = true;
        }
    }
}