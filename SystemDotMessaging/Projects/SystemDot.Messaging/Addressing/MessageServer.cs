using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Addressing
{
    public class MessageServer
    {
        public static MessageServer None { get { return new MessageServer(); } }

        public static MessageServer Named(string name, ServerAddress address)
        {
        
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(address != null);

            return new MessageServer(name, address.Address, address.IsSecure);
        }

        public string Name { get; set; }

        public string Address { get; set; }

        public bool IsSecure { get; set; }

        public MessageServer()
        {
        }

        MessageServer(string name, string address, bool isSecure)
        {
            Name = name;
            Address = address;
            IsSecure = isSecure;
        }

        protected bool Equals(MessageServer other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((MessageServer) obj);
        }

        public override string ToString()
        {
            return Name == null 
                ? "{NoServer}"
                : string.Concat(Name, " (", Address, ", ", IsSecure ? "Secure" : "Non Secure", ")");
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Name != null ? Name.GetHashCode() : 0;
            }
        }

        public static bool operator ==(MessageServer left, MessageServer right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MessageServer left, MessageServer right)
        {
            return !Equals(left, right);
        }
    }
}