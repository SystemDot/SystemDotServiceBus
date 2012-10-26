using SystemDot.Messaging.Channels.Addressing;

namespace SystemDot.Messaging.Channels.Packaging.Headers
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