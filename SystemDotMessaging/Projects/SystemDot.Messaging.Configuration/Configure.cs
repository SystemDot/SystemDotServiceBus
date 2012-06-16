using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.Configuration.Local;
using SystemDot.Messaging.Configuration.Remote;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration
{
    public class Configure
    {
        const int DefaultWorkerThreads = 4;

        static readonly ThreadPool threadPool = new ThreadPool(DefaultWorkerThreads);
        
        public static RemoteConfiguration Remote()
        {
            return new RemoteConfiguration(new ThreadedWorkCoordinator(new Threader()), threadPool);
        }

        public static LocalConfiguration Local()
        {
            return new LocalConfiguration(threadPool);
        }
    }
}