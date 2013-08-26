using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Authentication.Expiry
{
    class AuthenticationSessionExpiryStrategy : IMessageExpiryStrategy
    {
        readonly AuthenticatedServerRegistry registry;
        readonly MessageServer server;
        readonly ISystemTime systemTime;

        public AuthenticationSessionExpiryStrategy(AuthenticatedServerRegistry registry, MessageServer server, ISystemTime systemTime)
        {
            Contract.Requires(registry != null);
            Contract.Requires(server != null);
            Contract.Requires(systemTime != null);

            this.registry = registry;
            this.server = server;
            this.systemTime = systemTime;
        }

        public bool HasExpired(MessagePayload toCheck)
        {
            Logger.Debug("Checking expiry due to no or expired authentication: {0}", toCheck.Id);
            return registry.Contains(server) && (!toCheck.HasAuthenticationSession() || IsExpired(toCheck));
        }

        bool IsExpired(MessagePayload toCheck)
        {
            return toCheck.GetAuthenticationSession().ExpiresOn <= systemTime.GetCurrentDate();
        }
    }
}