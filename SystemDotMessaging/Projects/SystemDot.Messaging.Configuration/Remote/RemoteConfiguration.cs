using System.Diagnostics.Contracts;
using SystemDot.Messaging.Configuration.Local;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.Remote
{
    public class RemoteConfiguration
    {
        readonly ThreadedWorkCoordinator workCoordinator;
        readonly ThreadPool threadPool;

        public RemoteConfiguration(ThreadedWorkCoordinator workCoordinator, ThreadPool threadPool)
        {
            Contract.Requires(workCoordinator != null);
            Contract.Requires(threadPool != null);

            this.workCoordinator = workCoordinator;
            this.threadPool = threadPool;
        }

        public UsingDefaultsConfiguration UsingDefaults()
        {
            return new UsingDefaultsConfiguration(workCoordinator, threadPool);    
        }
    }
}