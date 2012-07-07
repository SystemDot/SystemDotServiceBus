using System;

namespace SystemDot.Parallelism
{
    public interface ITaskStarter
    {
        void StartTask(Action action);
    }
}