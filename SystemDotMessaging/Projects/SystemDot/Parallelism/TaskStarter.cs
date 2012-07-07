using System;
using System.Threading.Tasks;

namespace SystemDot.Parallelism
{
    public class TaskStarter : ITaskStarter
    {
        public void StartTask(Action action)
        {
            Task.Factory.StartNew(action);
        }
    }
}