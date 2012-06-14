using System;

namespace SystemDot.Threading
{
    public interface IThreader
    {
        void RunActionOnNewThread(Action toStart);
        void Stop();
    }
}