using System;

namespace SystemDot.Http
{
    public class FixedPortAddress
    {
        readonly ServerAddress address;
        readonly string instance;

        public string Url
        {
            get { return String.Concat(GetProtocol(), this.address.Address, ":8090/", this.instance, "/"); }
        }

        string GetProtocol()
        {
            return this.address.IsSecure ? "https://" : "http://";
        }

        public FixedPortAddress() {}

        public FixedPortAddress(ServerAddress address, string instance)
        {
            this.address = address;
            this.instance = instance;
        }

        public override string ToString()
        {
            return Url;
        }
    }
}