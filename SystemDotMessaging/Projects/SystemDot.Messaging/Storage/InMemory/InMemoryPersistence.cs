using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage.InMemory
{
    public class InMemoryPersistence : IPersistence
    {
        readonly ConcurrentDictionary<Guid, MessagePayload> messages;
        int sequence;
        
        public InMemoryPersistence()
        {
            this.messages = new ConcurrentDictionary<Guid, MessagePayload>();
            this.sequence = 1;
        }

        public IEnumerable<MessagePayload> GetMessages()
        {
            return this.messages.Values;
        }

        public void AddMessage(MessagePayload message)
        {
            this.messages.TryAdd(message.Id, message);
            this.sequence = this.sequence + 1;
        }

        public void UpdateMessage(MessagePayload message)
        {
            this.messages[message.Id] = message;
        }

        public void RemoveMessage(Guid id)
        {
            if (!this.messages.ContainsKey(id)) return;

            MessagePayload temp;
            this.messages.TryRemove(id, out temp);
        }

        public int GetSequence()
        {
            return this.sequence;
        }

        public void SetSequence(int toSet)
        {
            this.sequence = toSet;
        }
    }
}