using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Authentication.Expiry
{
    class AuthenticationSessionExpiryStrategy : IMessageExpiryStrategy
    {
        readonly AuthenticatedServerRegistry registry;
        readonly MessageServer server;

        public AuthenticationSessionExpiryStrategy(AuthenticatedServerRegistry registry, MessageServer server)
        {
            Contract.Requires(registry != null);
            Contract.Requires(server != null);

            this.registry = registry;
            this.server = server;
        }

        public bool HasExpired(MessagePayload toCheck)
        {
            return !toCheck.HasAuthenticationSession() && registry.Contains(server);
        }
    }
}