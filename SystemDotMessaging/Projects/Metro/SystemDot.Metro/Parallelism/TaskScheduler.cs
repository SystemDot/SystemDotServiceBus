using System;
using System.Threading.Tasks;
using SystemDot.Logging;

namespace SystemDot.Parallelism
{
    public class TaskScheduler : ITaskScheduler
    {
        public void ScheduleTask(TimeSpan delay, Action task)
        {
            Task.Run(async () =>
            {
                await Task.Delay(delay);
                DoTask(task);
            });
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