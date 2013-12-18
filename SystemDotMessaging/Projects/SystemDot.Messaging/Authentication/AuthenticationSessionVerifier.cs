using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Authentication
{
    abstract class AuthenticationSessionVerifier : MessageProcessor
    {
        readonly AuthenticationSessionCache cache;

        protected AuthenticationSessionVerifier(AuthenticationSessionCache cache)
        {
            Contract.Requires(cache != null);
            this.cache = cache;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            PerformVerifyingLogging(toInput);
            if (PayloadHasNoValidSession(toInput)) return;
            PerformVerifiedLogging(toInput);

            OnMessageProcessed(toInput);
        }

        static void PerformVerifyingLogging(MessagePayload toInput)
        {
            Logger.Debug("Verifying session for message: {0}", toInput.Id);
        }

        bool PayloadHasNoValidSession(MessagePayload toInput)
        {
            return ServerRequiresAuthentication(toInput) && !PayloadHasExpectedSession(toInput);
        }

        static void PerformVerifiedLogging(MessagePayload toInput)
        {
            Logger.Debug("Verified session for message: {0}", toInput.Id);
        }

        protected abstract bool ServerRequiresAuthentication(MessagePayload toInput);
        
        bool PayloadHasExpectedSession(MessagePayload toInput)
        {
            return toInput.HasAuthenticationSession() && cache.ContainsSession(toInput.GetAuthenticationSession());
        }
    }
}
