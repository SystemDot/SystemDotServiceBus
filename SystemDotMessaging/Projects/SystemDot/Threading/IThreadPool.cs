using System;

namespace SystemDot.Threading
{
    public interface IThreadPool
    {
        void QueueTask(Action action);
    }
}