using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Packaging.Headers
{
    public class FromAddressHeader : AddressHeader
    {
        public FromAddressHeader()
        {
        }

        public FromAddressHeader(EndpointAddress address) : base(address)
        {
        }
    }
}