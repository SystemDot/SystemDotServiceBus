using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Linq;

namespace SystemDot.Messaging.Packaging
{
    public class MessagePayload
    {
        public ConcurrentDictionary<string, IMessageHeader> Headers { get; set; }

        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public MessagePayload()
        {
            Headers = new ConcurrentDictionary<string, IMessageHeader>();
            Id = Guid.NewGuid();
            CreatedOn = DateTime.Now;
        }

        public void AddHeader(IMessageHeader toAdd)
        {
            Contract.Requires(toAdd != null);

            this.Headers.TryAdd(toAdd.GetType().Name, toAdd);
        }

        public void RemoveHeader(Type toRemove)
        {
            IMessageHeader temp;
            Headers.TryRemove(toRemove.Name, out temp);
        }

        public T GetHeader<T>()
        {
            return Headers.Values.Single(h => h.GetType() == typeof(T)).As<T>();
        }

        public bool HasHeader<T>()
        {
            return Headers.Values.Any(h => h.GetType() == typeof(T));
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
            return Equals((MessagePayload)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(MessagePayload left, MessagePayload right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MessagePayload left, MessagePayload right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return String.Concat("Message payload ", this.Id);
        }
    }
}