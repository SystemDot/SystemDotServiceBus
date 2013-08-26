using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Authentication
{
    class AuthenticationReceiveHook : IMessageHook<MessagePayload>
    {
        readonly AuthenticationSessionCache cache;

        public AuthenticationReceiveHook(AuthenticationSessionCache cache)
        {
            Contract.Requires(cache != null);
            this.cache = cache;
        }

        public void ProcessMessage(MessagePayload toInput, Action<MessagePayload> toPerformOnOutput)
        {
            if (!toInput.HasAuthenticationSession()) return;

            Logger.Debug("Caching session: {0} from authentication repsonse: {1}", toInput.GetAuthenticationSession().Id, toInput.Id);
                
            cache.CacheSessionFor(toInput.GetAuthenticationSession());
            toPerformOnOutput(toInput);
        }
    }
}