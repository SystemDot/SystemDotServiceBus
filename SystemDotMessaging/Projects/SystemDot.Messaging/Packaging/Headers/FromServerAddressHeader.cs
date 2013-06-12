namespace SystemDot.Messaging.Packaging.Headers
{
    public class FromServerAddressHeader : IMessageHeader 
    {
        public ServerAddress ServerAddress { get; set; }

        public FromServerAddressHeader()
        {
        }

        public FromServerAddressHeader(ServerAddress address)
        {
            this.ServerAddress = address;
        }

        public override string ToString()
        {
            return string.Concat("Address: ", this.ServerAddress);
        }
    }
}