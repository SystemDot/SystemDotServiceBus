using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Messages
{
    public class EndpointAddress
    {
        public static implicit operator EndpointAddress(string address)
        {
            return new EndpointAddress(address);
        }

        public static EndpointAddress Empty
        {
            get
            {
                return new EndpointAddress();
            }
        }

        public string Channel { get; set; }
        
        public string ServerName { get; set; }
        
        public string Address { get; set; }

        public EndpointAddress() {}

        public EndpointAddress(string address) : this()
        {
            Contract.Requires(!string.IsNullOrEmpty(address));

            Address = address;
            ServerName = string.Empty;

            var parts = address.Split('@', '.');
            Channel = parts[0];
            
            if (parts.Length == 3)
                ServerName = parts[2];
            
            if (parts.Length == 2)
                ServerName = parts[1];
            
        }

        public EndpointAddress(string channel, string machineName)
            : this(String.Concat(channel, "@", machineName))
        {
            Contract.Requires(!string.IsNullOrEmpty(channel));
            Contract.Requires(!string.IsNullOrEmpty(machineName));
        }

        public bool Equals(EndpointAddress other)
        {
            return other.Address == Address;
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
            return Address.GetHashCode();
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
            return String.Concat(Address, ".", ServerName);
        }
    }
}