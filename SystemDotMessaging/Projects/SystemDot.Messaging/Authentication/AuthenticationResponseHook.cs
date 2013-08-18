using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Direct;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;
using SystemDot.Serialisation;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Authentication
{
    public class AuthenticationResponseHook<TAuthenticationResponse> : IMessageHook<MessagePayload>
    {
        readonly ISerialiser serialiser;
        readonly AuthenticationSessionCache cache;

        public AuthenticationResponseHook(ISerialiser serialiser, AuthenticationSessionCache cache)
        {
            Contract.Requires(serialiser != null);
            Contract.Requires(cache != null);

            this.serialiser = serialiser;
            this.cache = cache;
        }

        public void ProcessMessage(MessagePayload toInput, Action<MessagePayload> toPerformOnOutput)
        {
            if(PayloadContainsAuthenticationResponse(toInput)) AddAuthenticationToPayload(toInput);
            toPerformOnOutput(toInput);
        }

        bool PayloadContainsAuthenticationResponse(MessagePayload toInput)
        {
            return serialiser.Deserialise(toInput.GetBody()).GetType() == typeof(TAuthenticationResponse);
        }

        void AddAuthenticationToPayload(MessagePayload toInput)
        {
            cache.CacheNewSessionFor(GetCurrentClientServer());
            toInput.SetAuthenticationSession(cache.GetCurrentSessionFor(GetCurrentClientServer()));
        }

        static MessageServer GetCurrentClientServer()
        {
            return DirectReplyContext.GetCurrentClientAddress().Server;
        }
    }
}