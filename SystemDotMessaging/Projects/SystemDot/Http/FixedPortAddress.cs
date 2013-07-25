using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Http
{
    public class FixedPortAddress
    {
        readonly string address;
        readonly bool isSecure;
        readonly string instance;

        public string Url
        {
            get { return String.Concat(GetProtocol(), this.address, "/", this.instance, "/"); }
        }

        string GetProtocol()
        {
            return this.isSecure ? "https://" : "http://";
        }

        public FixedPortAddress() {}

        public FixedPortAddress(ServerAddress address, string instance) : this(address.Path, address.IsSecure, instance)
        {
        }

        public FixedPortAddress(string address, bool isSecure, string instance)
        {
            Contract.Requires(!string.IsNullOrEmpty(address));
            Contract.Requires(!string.IsNullOrEmpty(instance));
            
            this.address = address;
            this.isSecure = isSecure;
            this.instance = instance;
        }

        public override string ToString()
        {
            return Url;
        }
    }
}