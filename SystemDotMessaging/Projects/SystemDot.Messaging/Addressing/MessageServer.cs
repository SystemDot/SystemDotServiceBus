using System;

namespace SystemDot.Messaging.Addressing
{
    public class MessageServer
    {
        public static MessageServer None { get { return new MessageServer(); } }

        public static MessageServer Local(string instance)
        {
            return Named(Environment.MachineName, instance); 
        }

        public static MessageServer Named(string name, string instance)
        {
            return new MessageServer(name, instance);
        }

        public string Name { get; set; }
        public string Instance { get; set; }
        
        public MessageServer()
        {
        }

        private MessageServer(string name, string instance)
        {
            Name = name;
            Instance = instance;
        }

        protected bool Equals(MessageServer other)
        {
            return string.Equals(Instance, other.Instance) && string.Equals(Name, other.Name);
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
            if (Name == null) return "{NoServer}";
            return String.Concat(Name, "/", Instance);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.Instance != null ? Instance.GetHashCode() : 0)*397) 
                    ^ (this.Name != null ? Name.GetHashCode() : 0);
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