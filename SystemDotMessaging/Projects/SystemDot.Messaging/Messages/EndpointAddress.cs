using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace SystemDot.Messaging.Messages
{
    [Serializable]
    public struct EndpointAddress
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

        public string ServerName { get; private set; }

        public string NodeName { get; private set; }
        
        public EndpointAddress(string address) : this()
        {
            Contract.Requires(!string.IsNullOrEmpty(address));

            ServerName = SplitAddress(address).Count() == 2 
                ? GetServer(address) 
                : string.Empty;

            NodeName = GetNode(address);
        }

        string GetServer(string address)
        {
            return SplitAddress(address).Last();
        }

        string GetNode(string address)
        {
            return SplitAddress(address).First();
        }

        static IEnumerable<string> SplitAddress(string address)
        {
            return address.Split('@');
        }

        public bool Equals(EndpointAddress other)
        {
            return other.ServerName == this.ServerName && other.NodeName == this.NodeName;
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
            return ServerName.GetHashCode() ^ NodeName.GetHashCode();
        }

        public static bool operator ==(EndpointAddress left, EndpointAddress right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EndpointAddress left, EndpointAddress right)
        {
            return !left.Equals(right);
        }
    }
}