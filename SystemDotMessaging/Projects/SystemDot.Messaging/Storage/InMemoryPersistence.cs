using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage
{
    public class InMemoryPersistence : IPersistence
    {
        readonly ConcurrentDictionary<Guid, Container> messages;
        readonly ConcurrentDictionary<EndpointAddress, int> sequences;

        public InMemoryPersistence()
        {
            this.messages = new ConcurrentDictionary<Guid, Container>();
            this.sequences = new ConcurrentDictionary<EndpointAddress, int>();
        }

        public IEnumerable<MessagePayload> GetMessages(EndpointAddress address)
        {
            return messages.Values
                .Where(m => m.Address == address)
                .Select(m => m.Payload);
        }

        public void AddMessage(MessagePayload message, EndpointAddress address)
        {
            messages.TryAdd(message.Id, new Container(message, address));
        }

        public void UpdateMessage(MessagePayload message)
        {
            messages[message.Id].Payload = message;
        }

        public void RemoveMessage(Guid id)
        {
            if (!messages.ContainsKey(id)) return;

            Container temp;
            messages.TryRemove(id, out temp);
        }

        public int GetNextSequence(EndpointAddress address)
        {
            return this.sequences[address];
        }

        public void InitialiseChannel(EndpointAddress address)
        {
            this.sequences.TryAdd(address, 1);
        }

        public void SetNextSequence(EndpointAddress address, int toSet)
        {
            this.sequences[address] = toSet;
        }

        class Container
        {
            public EndpointAddress Address { get; private set; }
            public MessagePayload Payload { get; set; }

            public Container(MessagePayload payload, EndpointAddress address)
            {
                Payload = payload;
                Address = address;
            }
        }
    }
}