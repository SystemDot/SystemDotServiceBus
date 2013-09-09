using System;
using System.Threading.Tasks;
using SystemDot.Logging;

namespace SystemDot.Parallelism
{
    public class TaskStarter : ITaskStarter
    {
        public Task StartTask(Action toStart)
        {
            return Task.Factory.StartNew(() => DoTask(toStart));
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