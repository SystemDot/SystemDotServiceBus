using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage
{
    public class Persistence : ChangeRoot, IPersistence
    {
        int sequence;
        readonly ConcurrentDictionary<Guid, MessagePayload> messages;

        public Persistence()
        {
            this.messages = new ConcurrentDictionary<Guid, MessagePayload>();
        }

        public IEnumerable<MessagePayload> GetMessages()
        {
            return this.messages.Values;
        }

        public void AddMessageAndIncrementSequence(MessagePayload message)
        {
            AddChange(new AddMessageAndIncrementSequenceChange(message, this.sequence + 1));
        }

        void ApplyChange(AddMessageAndIncrementSequenceChange change)
        {
            this.sequence = change.Sequence;
            this.messages.TryAdd(change.Message.Id, change.Message);
        }

        public void AddMessage(MessagePayload message)
        {
            throw new NotImplementedException();
        }

        public void UpdateMessage(MessagePayload message)
        {
            throw new NotImplementedException();
        }

        public int GetSequence()
        {
            throw new NotImplementedException();
        }

        public void SetSequence(int toSet)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public EndpointAddress Address { get; private set; }
        public PersistenceUseType UseType { get; private set; }
    
    }

    public class ChangeRoot
    {
        protected void AddChange(Change change)
        {
            throw new NotImplementedException();
        }
    }

    public class Change
    {
    }

    public class AddMessageAndIncrementSequenceChange : Change
    {
        public MessagePayload Message { get; set; }
        public int Sequence { get; set; }

        public AddMessageAndIncrementSequenceChange()
        {
        }

        public AddMessageAndIncrementSequenceChange(MessagePayload message, int sequence)
        {
            Message = message;
            Sequence = sequence;
        }
    }
}