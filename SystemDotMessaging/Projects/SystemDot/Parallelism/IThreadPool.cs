using System;

namespace SystemDot.Parallelism
{
    public interface IThreadPool
    {
        void QueueTask(Action action);
    }
}