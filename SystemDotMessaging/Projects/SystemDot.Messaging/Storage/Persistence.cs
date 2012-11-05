using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Storage
{
    public class Persistence : ChangeRoot, IPersistence
    {
        int sequence;
        readonly ConcurrentDictionary<Guid, MessagePayload> messages;
        readonly object deleteLock = new object();

        public Persistence(IChangeStore store, EndpointAddress address, PersistenceUseType useType)
            : base(store)
        {
            Address = address;
            UseType = useType;
            Id = address + "|" + useType;

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
            AddChange(new AddMessageChange(message));
        }

        void ApplyChange(AddMessageChange change)
        {
            this.messages.TryAdd(change.Message.Id, change.Message);
        }

        public void UpdateMessage(MessagePayload message)
        {
            AddChange(new UpdateMessageChange(message));
        }

        void ApplyChange(UpdateMessageChange change)
        {
            lock (deleteLock)
            {
                MessagePayload temp;
                if (this.messages.TryGetValue(change.Message.Id, out temp))
                    this.messages[change.Message.Id] = change.Message;
            }
        }

        public int GetSequence()
        {
            return this.sequence;
        }

        public void SetSequence(int toSet)
        {
            AddChange(new SetSequenceChange(toSet));
        }

        void ApplyChange(SetSequenceChange change)
        {
            this.sequence = change.Sequence;
        }

        public void Delete(Guid id)
        {
            AddChange(new DeleteMessageChange(id));
        }

        void ApplyChange(DeleteMessageChange change)
        {
            lock (deleteLock)
            {
                MessagePayload temp;
                this.messages.TryRemove(change.Id, out temp);
            }
        }

        public EndpointAddress Address { get; private set; }
        public PersistenceUseType UseType { get; private set; }

    }
}