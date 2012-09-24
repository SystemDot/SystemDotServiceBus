using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Channels
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

            Channel = channel;
            ServerName = serverName;
        }

        public bool Equals(EndpointAddress other)
        {
            return other.Channel == this.Channel;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) 
                return false;
            
            if (obj.GetType() != typeof(EndpointAddress)) 
                return false;
            
            return Equals((EndpointAddress)obj);
        }

        public override int GetHashCode()
        {
            return this.Channel.GetHashCode();
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
            return String.Concat(Channel, ".", ServerName);
        }
    }
}