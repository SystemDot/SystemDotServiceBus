using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Addressing;

namespace SystemDot.Messaging.Channels.Packaging.Headers
{
    public class AddressHeader : IMessageHeader
    {
        public EndpointAddress Address { get; set; }

        protected AddressHeader() { }

        protected AddressHeader(EndpointAddress address)
        {
            Contract.Requires(address != null);
            Address = address;
        }
    }
}