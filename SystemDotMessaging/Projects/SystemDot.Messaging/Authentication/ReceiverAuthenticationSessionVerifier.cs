using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Authentication
{
    class ReceiverAuthenticationSessionVerifier : AuthenticationSessionVerifier
    {
        readonly AuthenticatedServerRegistry registry;

        public ReceiverAuthenticationSessionVerifier(AuthenticationSessionCache cache, AuthenticatedServerRegistry registry)
            : base(cache)
        {
            Contract.Requires(registry != null);

            this.registry = registry;
        }

        protected override bool ServerRequiresAuthentication(MessagePayload toInput)
        {
            return registry.Contains(toInput.GetToAddress().Server);
        }
    }
}