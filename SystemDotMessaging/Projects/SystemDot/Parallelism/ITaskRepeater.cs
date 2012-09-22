using System;

namespace SystemDot.Parallelism
{
    public interface ITaskRepeater
    {
        void Register(TimeSpan delay, Action toLoop);
        void Start();
    }
}