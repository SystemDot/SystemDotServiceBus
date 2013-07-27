using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Addressing
{
    public class EndpointAddress
    {
        public static EndpointAddress Empty { get { return new EndpointAddress(); } }

        public string Channel { get; set; }

        public MessageServer Server { get; set; }
        
        public string OriginatingMachineName { get; set; }

        public EndpointAddress() {}

        public EndpointAddress(string channel, MessageServer server) 
        {
            Contract.Requires(!string.IsNullOrEmpty(channel));
            Contract.Requires(server != null);

            Channel = channel;
            Server = server;
            OriginatingMachineName = Environment.MachineName;
        }

        protected bool Equals(EndpointAddress other)
        {
            return string.Equals(OriginatingMachineName, other.OriginatingMachineName) && Equals(Server, other.Server) && string.Equals(Channel, other.Channel);
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
                int hashCode = (OriginatingMachineName != null ? OriginatingMachineName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Server != null ? Server.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Channel != null ? Channel.GetHashCode() : 0);
                return hashCode;
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
            return String.Concat(Channel, "@", Server);
        }
    }
}