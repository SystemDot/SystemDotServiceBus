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
        readonly AuthenticatedServerRegistry registry;

        protected AuthenticationSessionAttacher(AuthenticationSessionCache cache, AuthenticatedServerRegistry registry)
        {
            Contract.Requires(cache != null);
            Contract.Requires(registry != null);

            this.cache = cache;
            this.registry = registry;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            if (ServerRequiresAuthentication())
            {
                if (!IsCurrentSessionAvailable()) return;
                SetCurrentAuthenticationSessionOnPayload(toInput);
            }
            
            OnMessageProcessed(toInput);
        }

        bool ServerRequiresAuthentication()
        {
            return registry.Contains(GetServer());
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