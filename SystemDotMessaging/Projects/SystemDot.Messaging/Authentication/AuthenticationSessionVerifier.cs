using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

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
            if (ServerRequiresAuthentication(toInput) && !PayloadHasExpectedSession(toInput))
                return;

            OnMessageProcessed(toInput);
        }

        protected abstract bool ServerRequiresAuthentication(MessagePayload toInput);
        
        bool PayloadHasExpectedSession(MessagePayload toInput)
        {
            return toInput.HasAuthenticationSession() 
                && cache.GetCurrentSessionFor(toInput.GetFromAddress().Server) == toInput.GetAuthenticationSession();
        }
    }
}
