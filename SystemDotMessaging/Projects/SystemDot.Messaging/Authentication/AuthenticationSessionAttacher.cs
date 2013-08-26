using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Authentication
{
    class AuthenticationSessionAttacher : MessageProcessor
    {
        readonly AuthenticationSessionCache cache;
        readonly EndpointAddress address;

        public AuthenticationSessionAttacher(AuthenticationSessionCache cache, EndpointAddress address)
        {
            Contract.Requires(cache != null);
            Contract.Requires(address != null);
            this.cache = cache;
            this.address = address;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            if (cache.HasCurrentSessionFor(address.Server))
                toInput.SetAuthenticationSession(cache.GetCurrentSessionFor(address.Server));

            OnMessageProcessed(toInput);
        }
    }
}