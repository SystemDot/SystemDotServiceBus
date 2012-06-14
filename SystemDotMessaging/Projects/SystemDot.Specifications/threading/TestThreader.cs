using System;
using SystemDot.Threading;

namespace SystemDot.Specifications.threading
{
    public class TestThreader : IThreader 
    {
        public void RunActionOnNewThread(Action toStart)
        {
            toStart.Invoke();
            Threads++;
        }

        public void Stop()
        {
            this.Stopped = true;
        }

        public int Threads { get; private set; }

        public bool Stopped { get; private set; }
    }
}