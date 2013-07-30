using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Storage
{
    public class MessagePersistenceId : Equatable<MessagePersistenceId> 
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

        public override bool Equals(MessagePersistenceId other)
        {
            return MessageId.Equals(other.MessageId) 
                && Equals(Address, other.Address) 
                && UseType.Equals(other.UseType);
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

        public override string ToString()
        {
            return String.Concat(Address, " ", UseType);
        }
    }
}