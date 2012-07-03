using System;

namespace SystemDot.Parallelism
{
    public interface IThreader
    {
        void RunActionOnNewThread(Action toStart);
        void Stop();
    }
}