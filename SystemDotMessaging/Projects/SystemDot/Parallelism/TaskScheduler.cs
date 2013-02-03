using System;
using System.Timers;
using SystemDot.Logging;

namespace SystemDot.Parallelism
{
    public class TaskScheduler : ITaskScheduler 
    {
        public void ScheduleTask(TimeSpan delay, Action task)
        {
            var timer = new Timer(delay.TotalMilliseconds);
            timer.Elapsed += (sender, args) => DoTask(task);
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        static void DoTask(Action task)
        {
            try
            {
                task();
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
    }
}