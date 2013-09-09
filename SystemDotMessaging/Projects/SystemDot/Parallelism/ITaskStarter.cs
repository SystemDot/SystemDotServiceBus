using System;
using System.Threading.Tasks;

namespace SystemDot.Parallelism
{
    public interface ITaskStarter
    {
        Task StartTask(Action action);
    }
}