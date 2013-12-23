using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Authentication
{
    class SenderAuthenticationSessionVerifier : AuthenticationSessionVerifier
    {
        public SenderAuthenticationSessionVerifier(
            AuthenticationSessionCache cache, 
            AuthenticatedServerRegistry registry)
            : base(cache, registry)
        {
        }

        protected override EndpointAddress GetAddress(MessagePayload toInput)
        {
            return toInput.GetFromAddress();
        }
    }
}