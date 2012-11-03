using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Addressing;

namespace SystemDot.Messaging.Storage
{
    public class MessagePersistenceId
    {
        public Guid MessageId { get; set; }
        public EndpointAddress Address { get; set; }
        public PersistenceUseType UseType { get; set; }

        public MessagePersistenceId()
        {
        }

        public MessagePersistenceId(Guid id, EndpointAddress address, PersistenceUseType useType)
        {
            Contract.Requires(id != Guid.Empty);
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);
            
            MessageId = id;
            Address = address;
            UseType = useType;
        }

        protected bool Equals(MessagePersistenceId other)
        {
            return MessageId.Equals(other.MessageId) 
                && Equals(Address, other.Address) 
                && UseType.Equals(other.UseType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MessagePersistenceId) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = MessageId.GetHashCode();
                hashCode = (hashCode*397) ^ Address.GetHashCode();
                hashCode = (hashCode*397) ^ UseType.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(MessagePersistenceId left, MessagePersistenceId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MessagePersistenceId left, MessagePersistenceId right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return String.Concat(Address, " ", UseType);
        }
    }
}