namespace SystemDot.Messaging.Addressing
{
    public class SecureMessageServer : MessageServer
    {
        public SecureMessageServer(string name, string address)
            : base(name, address)
        {
        }

        public override string ToString()
        {
            return string.Concat(Name, " (", Address, ", ", "Secure", ")");
        }
    }
}