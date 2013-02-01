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

        public string ServerName { get; set; }
        
        public EndpointAddress() {}

        public EndpointAddress(string channel, string serverName) 
        {
            Contract.Requires(!string.IsNullOrEmpty(channel));
            Contract.Requires(!string.IsNullOrEmpty(serverName));

            this.Channel = channel;
            this.ServerName = serverName;
        }

        protected bool Equals(EndpointAddress other)
        {
            return string.Equals(this.Channel, other.Channel) 
                && string.Equals(this.ServerName, other.ServerName);
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
                return (this.Channel.GetHashCode()*397) ^ this.ServerName.GetHashCode();
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
            return String.Concat(this.Channel, ".", this.ServerName);
        }
    }
}