using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Addressing
{
    public class MessageServerBuilder
    {
        readonly ServerAddressRegistry serverAddressRegistry;

        public MessageServerBuilder(ServerAddressRegistry serverAddressRegistry)
        {
            Contract.Requires(serverAddressRegistry != null);
            this.serverAddressRegistry = serverAddressRegistry;
        }

        public MessageServer Build(string server)
        {
            Contract.Requires(!string.IsNullOrEmpty(server));
            return MessageServer.Named(server, serverAddressRegistry.Lookup(server));
        }
    }
}