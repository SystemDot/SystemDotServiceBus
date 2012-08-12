using System;
using System.Threading.Tasks;

namespace SystemDot.Parallelism
{
    public interface ITaskLooper 
    {
        void RegisterToLoop(Func<Task> toLoop);
        void Start();
    }
}