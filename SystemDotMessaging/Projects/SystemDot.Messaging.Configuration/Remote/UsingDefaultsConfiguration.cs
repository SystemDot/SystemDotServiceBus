using System.Diagnostics.Contracts;
using SystemDot.Messaging.Pipes;
using SystemDot.Messaging.Recieving;
using SystemDot.Messaging.Servers;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.Remote
{
    public class UsingDefaultsConfiguration
    {
        readonly ThreadedWorkCoordinator workCoordinator;
        readonly ThreadPool threadPool;

        public UsingDefaultsConfiguration(ThreadedWorkCoordinator workCoordinator, ThreadPool threadPool)
        {
            Contract.Requires(workCoordinator != null);
            Contract.Requires(threadPool != null);

            this.workCoordinator = workCoordinator;
            this.threadPool = threadPool;
        }

        public MessageHandlerConfiguration HandlingMessagesWith<T>(IMessageHandler<T> toRegister)
        {
            Contract.Requires(toRegister != null);

            return new MessageHandlerConfiguration(this.workCoordinator, this.threadPool, toRegister);
        }
    }
}