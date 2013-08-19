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

            return MessageServer.Named(server, GetAddress(server));
        }

        ServerAddress GetAddress(string server) { return serverAddressRegistry.Lookup(server); }

        public MessageServer BuildMultipoint(string client)
        {
            Contract.Requires(!string.IsNullOrEmpty(client));

            return MessageServer.NamedMultipoint(client, GetAddress(client));
        }
    }
}