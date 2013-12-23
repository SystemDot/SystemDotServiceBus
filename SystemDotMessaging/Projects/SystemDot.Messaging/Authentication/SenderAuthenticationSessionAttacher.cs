using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication.Caching;

namespace SystemDot.Messaging.Authentication
{
    class SenderAuthenticationSessionAttacher : AuthenticationSessionAttacher
    {
        readonly EndpointAddress address;

        public SenderAuthenticationSessionAttacher(
            AuthenticationSessionCache cache, 
            AuthenticatedServerRegistry registry,
            EndpointAddress address) : base(cache, registry)
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