using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Addressing
{
    public class MessageServer
    {
        public static MessageServer None { get { return new NullMessageServer(); } }

        public static MessageServer Named(string name, ServerAddress address)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(address != null);

            return address.IsSecure 
                ? new SecureMessageServer(name, address.Address) 
                : new MessageServer(name, address.Address);
        }

        public string Name { get; set; }

        public string Address { get; set; }

        public MessageServer()
        {
        }

        protected MessageServer(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public override string ToString()
        {
            return string.Concat(Name, " (", Address, ")");
        }

        protected bool Equals(MessageServer other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MessageServer) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
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