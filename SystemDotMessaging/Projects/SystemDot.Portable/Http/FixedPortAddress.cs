using System;

namespace SystemDot.Http
{
    public struct FixedPortAddress
    {
        readonly string server;

        public string Url
        {
            get { return String.Concat("http://", GetServer(), ":8090/"); }
        }

        string GetServer()
        {
            return (!string.IsNullOrEmpty(this.server)) ? this.server : "localhost";
        }

        public FixedPortAddress(string server)
        {
            this.server = server;
        }
    }
}