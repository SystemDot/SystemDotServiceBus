using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Authentication
{
    class AuthenticationSessionAttacher : MessageProcessor
    {
        readonly AuthenticationSessionCache cache;

        public AuthenticationSessionAttacher(AuthenticationSessionCache cache)
        {
            Contract.Requires(cache != null);
            this.cache = cache;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            if (cache.HasCurrentSessionFor(toInput.GetToAddress().Server))
                toInput.SetAuthenticationSession(cache.GetCurrentSessionFor(toInput.GetToAddress().Server));

            OnMessageProcessed(toInput);
        }
    }
}