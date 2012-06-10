using System.Diagnostics.Contracts;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.RequestReply.Server
{
    public class ServerConfiguration
    {
        readonly ThreadedWorkCoordinator coordinator;

        public ServerConfiguration(ThreadedWorkCoordinator coordinator)
        {
            Contract.Requires(coordinator != null);
            
            this.coordinator = coordinator;
        }

        public ChannelConfiguration OnChannelNamed(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            return new ChannelConfiguration(name, this.coordinator);
        }
    }
}