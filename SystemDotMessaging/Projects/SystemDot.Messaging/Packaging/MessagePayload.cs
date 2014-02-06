using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Core;

namespace SystemDot.Messaging.Packaging
{
    public class MessagePayload : Equatable<MessagePayload> 
    {
        public ConcurrentDictionary<string, IMessageHeader> Headers { get; set; }

        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public MessagePayload()
        {
            CreatedOn = SystemTime.Current.GetCurrentDate();
            Headers = new ConcurrentDictionary<string, IMessageHeader>();
            Id = Guid.NewGuid();
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

        public override bool Equals(MessagePayload other)
        {
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return String.Concat("Message payload ", Id);
        }
    }
}