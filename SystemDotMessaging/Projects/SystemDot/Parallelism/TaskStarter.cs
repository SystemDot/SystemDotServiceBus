using System;
using System.Threading.Tasks;
using SystemDot.Logging;

namespace SystemDot.Parallelism
{
    public class TaskStarter : ITaskStarter
    {
        public void StartTask(Action action)
        {
            Task.Factory.StartNew(() => DoTask(action));
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