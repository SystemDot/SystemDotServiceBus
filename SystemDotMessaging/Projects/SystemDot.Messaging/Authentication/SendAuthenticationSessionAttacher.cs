using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication.Caching;

namespace SystemDot.Messaging.Authentication
{
    class SendAuthenticationSessionAttacher : AuthenticationSessionAttacher
    {
        readonly EndpointAddress address;

        public SendAuthenticationSessionAttacher(AuthenticationSessionCache cache, EndpointAddress address) : base(cache)
        {
            Contract.Requires(address != null);
            this.address = address;
        }

        protected override EndpointAddress GetAddress()
        {
            return address;
        }
    }
}