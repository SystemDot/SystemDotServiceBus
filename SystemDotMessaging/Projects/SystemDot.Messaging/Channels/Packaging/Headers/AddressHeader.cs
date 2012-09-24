using System.Diagnostics.Contracts;

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