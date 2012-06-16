using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.MessageTransportation
{
    [Serializable]
    public struct Address
    {
        public static Address Default
        {
            get
            {
                return new Address("http://localhost:8090/Default/");
            }
        }

        public static Address Empty
        {
            get
            {
                return new Address();
            }
        }

        public string Url { get; set; }
        
        public Address(string url) : this()
        {
            Contract.Requires(!string.IsNullOrEmpty(url));
            Url = url;
        }

        public bool Equals(Address other)
        {
            return other.Url == this.Url;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) 
                return false;
            
            if (obj.GetType() != typeof(Address)) 
                return false;
            
            return Equals((Address)obj);
        }

        public override int GetHashCode()
        {
            return Url.GetHashCode();
        }

        public static bool operator ==(Address left, Address right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Address left, Address right)
        {
            return !left.Equals(right);
        }
    }
}