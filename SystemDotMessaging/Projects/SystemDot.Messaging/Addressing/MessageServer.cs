using System;

namespace SystemDot.Messaging.Addressing
{
    public class MessageServer
    {
        public static MessageServer None { get { return new MessageServer(); } }

        public static MessageServer Named(string name)
        {
            return new MessageServer(name);
        }

        public string Name { get; set; }
        
        public MessageServer()
        {
        }

        private MessageServer(string name)
        {
            Name = name;
        }

        protected bool Equals(MessageServer other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MessageServer) obj);
        }

        public override string ToString()
        {
            return Name ?? "{NoServer}";
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