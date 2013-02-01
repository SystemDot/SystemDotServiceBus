using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Packaging.Headers
{
    public class ToAddressHeader : AddressHeader 
    {
        public ToAddressHeader()
        {
        }

        public ToAddressHeader(EndpointAddress address) : base(address)
        {
        }
    }
}