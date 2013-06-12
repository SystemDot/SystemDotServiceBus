using System;

namespace SystemDot
{
    public class ServerAddress
    {
        public static ServerAddress Local { get { return new ServerAddress(Environment.MachineName, false); } }
        
        public string Address { get; set; }
        
        public bool IsSecure { get; set; }

        public ServerAddress()
        {
        }

        public ServerAddress(string address, bool isSecure)
        {
            Address = address;
            IsSecure = isSecure;
        }

        public override string ToString()
        {
            return String.Concat(Address, ", ", IsSecure ? "Secure" : "Non Secure");
        }
    }
}