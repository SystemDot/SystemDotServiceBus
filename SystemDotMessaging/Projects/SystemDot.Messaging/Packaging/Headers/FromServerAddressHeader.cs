namespace SystemDot.Messaging.Packaging.Headers
{
    public class FromServerAddressHeader : IMessageHeader 
    {
        public string Address { get; set; }

        public FromServerAddressHeader()
        {
        }

        public FromServerAddressHeader(string address)
        {
            Address = address;
        }

        public override string ToString()
        {
            return string.Concat("Address:", Address);
        }
    }
}