using System;
using System.Collections.Generic;
using System.Threading;

namespace SystemDot.Threading
{
    public class Threader : IThreader 
    {
        bool stopped;
        readonly List<Thread> threads;

        public Threader()
        {
            this.threads = new List<Thread>();
        }

        public void RunActionOnNewThread(Action toStart)
        {
            var thread = new Thread(() => DoWork(toStart));
            thread.Start();
            threads.Add(thread);
        }

        void DoWork(Action toStart)
        {
            while(!this.stopped)
                toStart.Invoke();
        }

        public void Stop()
        {
            this.stopped = true;
            this.threads.ForEach(t => t.Join());
        }
    }
}