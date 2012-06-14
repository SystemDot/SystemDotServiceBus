using SystemDot.Messaging.Configuration.Local;
using SystemDot.Messaging.Configuration.Remote;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration
{
    public class Configure
    {
        const int DefaultWorkerThreads = 4;

        static readonly ThreadPool threadPool = new ThreadPool(DefaultWorkerThreads);
        static readonly ThreadedWorkCoordinator WorkCoordinator = new ThreadedWorkCoordinator(new Threader());

        public static RemoteConfiguration Remote()
        {
            return new RemoteConfiguration(WorkCoordinator, threadPool);
        }

        public static LocalConfiguration Local()
        {
            return new LocalConfiguration(threadPool);
        }

    }
}