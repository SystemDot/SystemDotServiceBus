using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Authentication
{
    abstract class AuthenticationSessionAttacher : MessageProcessor
    {
        readonly AuthenticationSessionCache cache;
        
        protected AuthenticationSessionAttacher(AuthenticationSessionCache cache)
        {
            Contract.Requires(cache != null);
            this.cache = cache;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            if (ServerRequiresAuthentication() && !IsCurrentSessionAvailable()) return;
            SetCurrentAuthenticationSessionOnPayload(toInput);
            OnMessageProcessed(toInput);
        }

        bool ServerRequiresAuthentication()
        {
            return cache.
        }

        bool IsCurrentSessionAvailable()
        {
            return cache.HasCurrentSessionFor(GetServer());
        }

        void SetCurrentAuthenticationSessionOnPayload(MessagePayload toInput)
        {
            PerformLogging(toInput);
            toInput.SetAuthenticationSession(cache.GetCurrentSessionFor(GetServer()));
        }

        void PerformLogging(MessagePayload toInput)
        {
            Logger.Debug(
                "Attaching session {0} for message: {0}", 
                cache.GetCurrentSessionFor(GetServer()).Id, 
                toInput.Id);
        }

        MessageServer GetServer()
        {
            return GetAddress().Server;
        }

        protected abstract EndpointAddress GetAddress();
    }
}