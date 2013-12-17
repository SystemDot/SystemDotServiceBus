using System;
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
            Logger.Debug("Verifying session for message: {0}", toInput.Id);
                
            if (ServerRequiresAuthentication(toInput) && !PayloadHasExpectedSession(toInput))
                throw new MessageSessionExpiredException(toInput);

            Logger.Debug("Verified session for message: {0}", toInput.Id);

            OnMessageProcessed(toInput);
        }

        protected abstract bool ServerRequiresAuthentication(MessagePayload toInput);
        
        bool PayloadHasExpectedSession(MessagePayload toInput)
        {
            return toInput.HasAuthenticationSession() && cache.ContainsSession(toInput.GetAuthenticationSession());
        }
    }

    public class MessageSessionExpiredException : Exception
    {
        public MessageSessionExpiredException(MessagePayload payload) 
            : base(String.Format("Message {0} was not handled because its session has expired", payload.Id))
        {
        }
    }
}
