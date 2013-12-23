using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication.Caching;

namespace SystemDot.Messaging.Authentication
{
    class SenderAuthenticationSessionAttacherFactory
    {
        readonly AuthenticationSessionCache cache;
        readonly AuthenticatedServerRegistry registry;

        public SenderAuthenticationSessionAttacherFactory(
            AuthenticationSessionCache cache,
            AuthenticatedServerRegistry registry)
        {
            this.cache = cache;
            this.registry = registry;
            Contract.Requires(cache != null);
            Contract.Requires(registry != null);
        }

        public SenderAuthenticationSessionAttacher Create(EndpointAddress address)
        {
            return new SenderAuthenticationSessionAttacher(cache, registry, address);
        }
    }
}