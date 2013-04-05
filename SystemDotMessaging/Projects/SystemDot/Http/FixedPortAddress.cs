using System;

namespace SystemDot.Http
{
    public class FixedPortAddress
    {
        readonly string machineName;
        readonly string instance;

        public string Url
        {
            get { return String.Concat("http://", this.machineName, ":8090/", this.instance, "/"); }
        }

        public FixedPortAddress() {}

        public FixedPortAddress(string machineName, string instance)
        {
            this.machineName = machineName;
            this.instance = instance;
        }

        public override string ToString()
        {
            return Url;
        }
    }
}