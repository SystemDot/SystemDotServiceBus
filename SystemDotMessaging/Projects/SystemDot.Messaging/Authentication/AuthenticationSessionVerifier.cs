using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Authentication
{
    abstract class AuthenticationSessionVerifier : MessageProcessor
    {
        readonly AuthenticationSessionCache cache;
        readonly AuthenticatedServerRegistry registry;

        protected AuthenticationSessionVerifier(
            AuthenticationSessionCache cache, 
            AuthenticatedServerRegistry registry)
        {
            Contract.Requires(cache != null);
            Contract.Requires(registry != null);
            this.cache = cache;
            this.registry = registry;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            PerformVerifyingLogging(toInput);

            if (ServerRequiresAuthentication(toInput))
            {
                RegisterFromServerAsAuthenticated(toInput);
                if (!PayloadHasExpectedSession(toInput)) return;
            }

            PerformVerifiedLogging(toInput);

            OnMessageProcessed(toInput);
        }

        void RegisterFromServerAsAuthenticated(MessagePayload toInput)
        {
            registry.Register(toInput.GetFromAddress().Server);
        }

        static void PerformVerifyingLogging(MessagePayload toInput)
        {
            Logger.Debug("Verifying session for message: {0}", toInput.Id);
        }

        static void PerformVerifiedLogging(MessagePayload toInput)
        {
            Logger.Debug("Verified session for message: {0}", toInput.Id);
        }

        bool ServerRequiresAuthentication(MessagePayload toInput)
        {
            return registry.Contains(GetAddress(toInput).Server);
        }

        protected abstract EndpointAddress GetAddress(MessagePayload toInput);
        
        bool PayloadHasExpectedSession(MessagePayload toInput)
        {
            return toInput.HasAuthenticationSession() && cache.ContainsSession(toInput.GetAuthenticationSession());
        }
    }
}
