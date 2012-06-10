using System.Collections.Generic;

namespace SystemDot.Threading
{
    public class ThreadedWorkCoordinator : IWorkCoordinator
    {
        readonly int workerThreads;
        readonly IThreader threader;
        readonly List<IWorker> workers;
        bool isRunning;
        
        public ThreadedWorkCoordinator(int workerThreads, IThreader threader)
        {
            this.workerThreads = workerThreads;
            this.threader = threader;
            this.workers = new List<IWorker>();
        }

        void PerformWork()
        {
            do
            {
                this.workers.ForEach(p => p.PerformWork());
            } 
            while (this.isRunning);
        }

        public void Start()
        {
            this.workers.ForEach(p => p.OnWorkStarted());
        
            for (int i = 0; i < this.workerThreads; i++)
            {
                this.threader.Start(PerformWork);
            }
            this.isRunning = true;
        }

        public void RegisterWorker(IWorker worker)
        {
            this.workers.Add(worker);
        }

        public void Dispose()
        {
            this.workers.ForEach(p => p.OnWorkStopped());
            this.isRunning = false;
        }
    }
}