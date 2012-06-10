using System.Diagnostics.Contracts;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.RequestReply.Client
{
    public class ClientConfiguration
    {
        readonly ThreadedWorkCoordinator coordinator;

        public ClientConfiguration(ThreadedWorkCoordinator coordinator)
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