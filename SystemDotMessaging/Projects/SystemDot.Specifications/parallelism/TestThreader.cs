using System;
using SystemDot.Parallelism;

namespace SystemDot.Specifications.parallelism
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