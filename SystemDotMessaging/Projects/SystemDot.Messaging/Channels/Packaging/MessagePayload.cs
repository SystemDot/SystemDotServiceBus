using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Packaging
{
    public class MessagePayload
    {
        public List<IMessageHeader> Headers { get; set; }

        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public MessagePayload()
        {
            Headers = new List<IMessageHeader>();
            Id = Guid.NewGuid();
            CreatedOn = DateTime.Now;
        }

        public void AddHeader(IMessageHeader toAdd)
        {
            Contract.Requires(toAdd != null);

            this.Headers.Add(toAdd);
        }

        public void RemoveHeader(PersistenceHeader toRemove)
        {
            RemoveHeader(toRemove.GetType());
        }

        public void RemoveHeader(Type toRemove)
        {
            this.Headers.RemoveAll(h => h.GetType() == toRemove);
        }

        public T GetHeader<T>()
        {
            return this.Headers.Single(h => h.GetType() == typeof(T)).As<T>();
        }

        public bool HasHeader<T>()
        {
            return this.Headers.Any(h =>  h.GetType() == typeof (T));
        }

        protected bool Equals(MessagePayload other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MessagePayload) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return String.Concat("Message payload ", this.Id);
        }
    }
}