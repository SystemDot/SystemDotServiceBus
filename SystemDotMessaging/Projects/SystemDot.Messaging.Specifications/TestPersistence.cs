using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications
{
    public class TestPersistence : IPersistence
    {
        readonly InMemoryPersistence inner;
        readonly List<EndpointAddress> initialisedChannels;

        public TestPersistence()
        {
            this.inner = new InMemoryPersistence();
            this.initialisedChannels = new List<EndpointAddress>();
        }

        public MessagePayload StoredMessage { get; private set; }

        public IEnumerable<MessagePayload> GetMessages(EndpointAddress address)
        {
            if (!this.initialisedChannels.Contains(address)) throw new InvalidOperationException();
            return this.inner.GetMessages(address);
        }

        public void AddMessage(MessagePayload message, EndpointAddress address)
        {
            if (!this.initialisedChannels.Contains(address)) throw new InvalidOperationException();
            if (this.inner.GetMessages(address).Any(m => m.Id == message.Id)) throw new InvalidOperationException();
            this.inner.AddMessage(message, address);
            StoredMessage = message;
        }

        public void UpdateMessage(MessagePayload message)
        {
            this.inner.UpdateMessage(message);
        }

        public void RemoveMessage(Guid id)
        {
            this.inner.RemoveMessage(id);
        }

        public int GetNextSequence(EndpointAddress address)
        {
            if (!this.initialisedChannels.Contains(address)) throw new InvalidOperationException();
            return this.inner.GetNextSequence(address);
        }

        public void InitialiseChannel(EndpointAddress address)
        {
            this.initialisedChannels.Add(address);
            this.inner.InitialiseChannel(address);
        }
    }
}