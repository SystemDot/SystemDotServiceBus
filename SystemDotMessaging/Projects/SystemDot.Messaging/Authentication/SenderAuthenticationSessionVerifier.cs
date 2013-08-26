using System.Diagnostics.Contracts;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Authentication
{
    class SenderAuthenticationSessionVerifier : AuthenticationSessionVerifier
    {
        readonly AuthenticatedServerRegistry registry;

        public SenderAuthenticationSessionVerifier(AuthenticationSessionCache cache, AuthenticatedServerRegistry registry)
            : base(cache)
        {
            Contract.Requires(registry != null);
            Contract.Requires(cache != null);

            this.registry = registry;
        }

        protected override bool ServerRequiresAuthentication(MessagePayload toInput)
        {
            return registry.Contains(toInput.GetFromAddress().Server);
        }
    }
}