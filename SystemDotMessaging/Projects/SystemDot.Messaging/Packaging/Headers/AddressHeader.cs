using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Packaging.Headers
{
    public class AddressHeader : IMessageHeader
    {
        public EndpointAddress Address { get; set; }

        protected AddressHeader() { }

        protected AddressHeader(EndpointAddress address)
        {
            Contract.Requires(address != null);
            this.Address = address;
        }

        public override string ToString()
        {
            return string.Concat("Address: ", Address.ToString());
        }
    }
}