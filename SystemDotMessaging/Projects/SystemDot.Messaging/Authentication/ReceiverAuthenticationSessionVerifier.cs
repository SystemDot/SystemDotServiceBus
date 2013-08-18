using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Authentication
{
    class ReceiverAuthenticationSessionVerifier : AuthenticationSessionVerifier
    {
        readonly AuthenticatedServerRegistry registry;
        readonly InvalidAuthenticationSessionNotifier notifier;

        public ReceiverAuthenticationSessionVerifier(AuthenticationSessionCache cache, AuthenticatedServerRegistry registry, InvalidAuthenticationSessionNotifier notifier)
            : base(cache)
        {
            Contract.Requires(registry != null);
            Contract.Requires(notifier != null);

            this.registry = registry;
            this.notifier = notifier;
        }

        protected override void NotifyInvalidAuthenticationSession(MessagePayload toInput)
        {
            notifier.Notify(toInput);
        }

        protected override bool ServerRequiresAuthentication(MessagePayload toInput)
        {
            return registry.Contains(toInput.GetToAddress().Server);
        }
    }
}