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

        public MessageServer BuildOutbound(string server)
        {
            Contract.Requires(!string.IsNullOrEmpty(server));

            return MessageServer.Outbound(server, GetAddress(server));
        }

        public MessageServer BuildInbound(string server)
        {
            Contract.Requires(!string.IsNullOrEmpty(server));

            return MessageServer.Inbound(server, GetAddress(server));
        }

        ServerAddress GetAddress(string server) { return serverAddressRegistry.Lookup(server); }
    }
}