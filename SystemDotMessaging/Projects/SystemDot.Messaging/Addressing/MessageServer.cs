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

            return new MessageServer(name, address);
        }

        public string Name { get; set; }

        public ServerAddress Address { get; set; }

        public MessageServer()
        {
        }

        MessageServer(string name, ServerAddress address)
        {
            Name = name;
            Address = address;
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
                : string.Concat(Name, " (", Address, ")");
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