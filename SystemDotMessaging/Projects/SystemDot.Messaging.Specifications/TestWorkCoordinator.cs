using System.Collections.Generic;
using SystemDot.Threading;

namespace SystemDot.Messaging.Specifications
{
    public class TestWorkCoordinator : IWorkCoordinator
    {
        readonly List<IWorker> performers;
        
        public TestWorkCoordinator()
        {
            this.performers = new List<IWorker>();
        }

        public bool IsRunning { get; private set; }

        public void Start()
        {
            performers.ForEach(x => x.PerformWork());
            IsRunning = true;
        }

        public void RegisterWorker(IWorker worker)
        {
            this.performers.Add(worker);
        }

        public void Dispose()
        {
            IsRunning = false;
        }
    }
}