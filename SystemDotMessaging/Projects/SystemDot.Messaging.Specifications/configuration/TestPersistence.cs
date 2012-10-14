using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;

namespace SystemDot.Messaging.Specifications.configuration
{
    public class TestPersistence : IPersistence
    {
        private readonly IPersistence inner;

        public MessagePayload LastAddedMessage { get; private set; }
        
        public TestPersistence()
        {
            this.inner = new InMemoryPersistence();
        }

        public IEnumerable<MessagePayload> GetMessages()
        {
            return this.inner.GetMessages();
        }

        public void AddMessage(MessagePayload message)
        {
            this.LastAddedMessage = message;
            this.inner.AddMessage(message);
        }

        public void UpdateMessage(MessagePayload message)
        {
            this.inner.UpdateMessage(message);
        }

        public void RemoveMessage(Guid id)
        {
            this.inner.RemoveMessage(id);
        }

        public int GetSequence()
        {
            return this.inner.GetSequence();
        }

        public void SetSequence(int toSet)
        {
            this.inner.SetSequence(toSet);
        }
    }
}