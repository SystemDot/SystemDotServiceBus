namespace SystemDot.Messaging.Messages.Packaging.Headers
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