using System;
using System.Diagnostics.Contracts;
using SystemDot.Core;

namespace SystemDot.Messaging.Addressing
{
    public class EndpointAddress : Equatable<EndpointAddress> 
    {
        public static EndpointAddress Empty { get { return new EndpointAddress(); } }

        public string Channel { get; set; }

        public MessageServer Server { get; set; }
        
        public EndpointAddress() {}

        public EndpointAddress(string channel, MessageServer server) 
        {
            Contract.Requires(!string.IsNullOrEmpty(channel));
            Contract.Requires(server != null);

            Channel = channel;
            Server = server;
        }

        public override bool Equals(EndpointAddress other)
        {
            return Equals(Server, other.Server) && string.Equals(Channel, other.Channel);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Server != null ? Server.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Channel != null ? Channel.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return String.Concat(Channel, "@", Server);
        }
    }
}