using System;

namespace SystemDot.Http
{
    public class FixedPortAddress
    {
        readonly string machineName;

        public string Url
        {
            get { return String.Concat("http://", this.machineName, ":8090/"); }
        }

        public FixedPortAddress() {}

        public FixedPortAddress(string machineName)
        {
            this.machineName = machineName;
        }

        public override string ToString()
        {
            return Url;
        }
    }
}