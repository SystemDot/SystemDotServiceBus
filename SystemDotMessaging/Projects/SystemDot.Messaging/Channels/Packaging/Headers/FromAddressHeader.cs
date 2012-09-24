namespace SystemDot.Messaging.Channels.Packaging.Headers
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