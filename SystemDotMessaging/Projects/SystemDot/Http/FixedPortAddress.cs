using System;

namespace SystemDot.Http
{
    public class FixedPortAddress
    {
        readonly string address;
        readonly string instance;

        public string Url
        {
            get { return String.Concat("http://", this.address, ":8090/", this.instance, "/"); }
        }

        public FixedPortAddress() {}

        public FixedPortAddress(string address, string instance)
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