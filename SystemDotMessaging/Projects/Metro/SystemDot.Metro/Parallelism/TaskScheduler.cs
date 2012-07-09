using System;
using System.Threading.Tasks;

namespace SystemDot.Parallelism
{
    public class TaskScheduler : ITaskScheduler
    {
        
        public void ScheduleTask(TimeSpan delay, Action task)
        {
            Task.Run(() =>
            {
                Task.Delay(delay);
                task();
            });
        }
    }
}