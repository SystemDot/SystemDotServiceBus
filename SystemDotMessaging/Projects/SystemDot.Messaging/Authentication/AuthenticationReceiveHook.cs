using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Authentication
{
    public class AuthenticationReceiveHook : IMessageHook<MessagePayload>
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

            cache.CacheSessionFor(toInput.GetFromAddress().Server, toInput.GetAuthenticationSession());
            toPerformOnOutput(toInput);
        }
    }
}