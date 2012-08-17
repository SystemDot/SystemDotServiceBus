namespace SystemDot.Messaging.Messages.Packaging.Headers
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