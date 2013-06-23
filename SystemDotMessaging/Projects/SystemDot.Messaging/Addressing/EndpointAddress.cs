using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Addressing
{
    public class EndpointAddress
    {
        public static EndpointAddress Empty
        {
            get
            {
                return new EndpointAddress();
            }
        }

        public string Channel { get; set; }

        public ServerPath ServerPath { get; set; }

        public EndpointAddress() {}

        public EndpointAddress(string channel, ServerPath serverPath) 
        {
            Contract.Requires(!string.IsNullOrEmpty(channel));
            Contract.Requires(serverPath != null);

            this.Channel = channel;
            this.ServerPath = serverPath;
        }

        protected bool Equals(EndpointAddress other)
        {
            return string.Equals(Channel, other.Channel) 
                && string.Equals(ServerPath, other.ServerPath);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EndpointAddress) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Channel.GetHashCode()*397) ^ ServerPath.GetHashCode();
            }
        }

        public static bool operator ==(EndpointAddress left, EndpointAddress right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EndpointAddress left, EndpointAddress right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return String.Concat(Channel, "@", ServerPath);
        }
    }
}