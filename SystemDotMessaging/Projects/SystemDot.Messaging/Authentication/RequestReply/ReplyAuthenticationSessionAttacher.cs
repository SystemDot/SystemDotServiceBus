using System.Threading;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Authentication.RequestReply
{
    class ReplyAuthenticationSessionAttacher : AuthenticationSessionAttacher
    {
        readonly ThreadLocal<EndpointAddress> address;

        public ReplyAuthenticationSessionAttacher(
            AuthenticationSessionCache cache,
            AuthenticatedServerRegistry registry)
            : base(cache, registry)
        {
            address = new ThreadLocal<EndpointAddress>();
        }

        public override void InputMessage(MessagePayload toInput)
        {
            address.Value = toInput.GetToAddress();
            base.InputMessage(toInput);
        }

        protected override EndpointAddress GetAddress()
        {
            return address.Value;
        }
    }
}