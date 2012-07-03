using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SystemDot.Parallelism
{
    public class AsynchronousWorkCoordinator : Disposable
    {
        readonly IThreader threader;
        readonly List<IWorker> workers;
        bool isStarted;

        public AsynchronousWorkCoordinator(IThreader threader)
        {
            Contract.Requires(threader != null);
            
            this.threader = threader;
            this.workers = new List<IWorker>();
        }

        public void Start()
        {
            this.isStarted = true;
            this.workers.ForEach(StartWorkOnWorker);
        }

        public void RegisterWorker(IWorker worker)
        {
            Contract.Requires(worker != null);

            this.workers.Add(worker);
            if(this.isStarted) StartWorkOnWorker(worker);
        }

        void StartWorkOnWorker(IWorker worker)
        {
            worker.StartWork();
            this.threader.RunActionOnNewThread(worker.PerformWork);
        }

        protected override void DisposeOfManagedResources()
        {
            this.workers.ForEach(w => w.StopWork());
            this.threader.Stop();

            base.DisposeOfManagedResources();
        }
    }
}