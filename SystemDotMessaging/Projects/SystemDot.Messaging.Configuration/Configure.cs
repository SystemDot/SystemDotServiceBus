using SystemDot.Messaging.Configuration.RequestReply.Client;
using SystemDot.Messaging.Configuration.RequestReply.Server;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration
{
    public class Configure
    {
        const int DefaultWorkerThreads = 4;

        public static ClientConfiguration RequestReplyClient()
        {
            return new ClientConfiguration(GetThreadedWorkCoordinator());
        }

        public static ServerConfiguration RequestReplyServer()
        {
            return new ServerConfiguration(GetThreadedWorkCoordinator());
        }

        static ThreadedWorkCoordinator GetThreadedWorkCoordinator()
        {
            return new ThreadedWorkCoordinator(DefaultWorkerThreads, new Threader());
        }
    }
}