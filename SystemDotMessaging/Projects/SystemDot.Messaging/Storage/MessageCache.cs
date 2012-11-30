using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Storage
{
    public class MessageCache : ChangeRoot
    {
        int sequence;
        readonly ConcurrentDictionary<Guid, MessagePayload> messages;
        readonly object deleteLock = new object();

        public EndpointAddress Address { get; private set; }
        public PersistenceUseType UseType { get; private set; }

        public MessageCache(IChangeStore changeStore, EndpointAddress address, PersistenceUseType useType)
            : base(changeStore)
        {
            Address = address;
            UseType = useType;
            Id = address + "|" + useType;
            
            this.messages = new ConcurrentDictionary<Guid, MessagePayload>();
            this.sequence = 1;           
        }

        public IEnumerable<MessagePayload> GetMessages()
        {
            return this.messages.Values;
        }

        public void AddMessageAndIncrementSequence(MessagePayload message)
        {
            AddChange(new AddMessageAndIncrementSequenceChange(message, this.sequence + 1));
        }

        public void ApplyChange(AddMessageAndIncrementSequenceChange change)
        {
            this.sequence = change.Sequence;
            this.messages.TryAdd(change.Message.Id, change.Message);
        }

        public void AddMessage(MessagePayload message)
        {
            AddChange(new AddMessageChange(message));
        }

        public void ApplyChange(AddMessageChange change)
        {
            this.messages.TryAdd(change.Message.Id, change.Message);
        }

        public void UpdateMessage(MessagePayload message)
        {
            AddChange(new UpdateMessageChange(message));
        }

        public void ApplyChange(UpdateMessageChange change)
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

        public void ApplyChange(SetSequenceChange change)
        {
            this.sequence = change.Sequence;
        }

        public void Delete(Guid id)
        {
            AddChange(new DeleteMessageChange(id));
        }

        public void ApplyChange(DeleteMessageChange change)
        {
            ApplyDelete(change.Id);
        }

        public void DeleteAndSetSequence(Guid id, int toSet)
        {
            AddChange(new DeleteMessageAndSetSequenceChange(id, toSet));
        }

        public void ApplyChange(DeleteMessageAndSetSequenceChange change)
        {
            ApplyDelete(change.Id);
            this.sequence = change.Sequence;
        }

        void ApplyDelete(Guid id)
        {
            lock (deleteLock)
            {
                MessagePayload temp;
                this.messages.TryRemove(id, out temp);
            }
        }

        protected override void UrgeCheckPoint()
        {
            if (this.messages.Count == 0)
                CheckPoint(new MessageCheckpointChange { Sequence = this.sequence });
        }

        public void ApplyChange(MessageCheckpointChange change)
        {
            this.sequence = change.Sequence;
        }
    }
}