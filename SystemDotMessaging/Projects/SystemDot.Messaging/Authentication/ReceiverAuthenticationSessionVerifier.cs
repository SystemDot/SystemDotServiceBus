using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Authentication
{
    class ReceiverAuthenticationSessionVerifier : AuthenticationSessionVerifier
    {
        public ReceiverAuthenticationSessionVerifier(
            AuthenticationSessionCache cache, 
            AuthenticatedServerRegistry registry)
            : base(cache, registry)
        {
        }

        protected override EndpointAddress GetAddress(MessagePayload toInput)
        {
            return toInput.GetToAddress();
        }
    }
}