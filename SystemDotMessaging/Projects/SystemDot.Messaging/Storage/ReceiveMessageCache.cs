using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Storage
{
    class ReceiveMessageCache : ChangeRoot, IMessageCache
    {
        int sequence;
        readonly ConcurrentDictionary<Guid, MessagePayload> messages;
        
        public EndpointAddress Address { get; private set; }
        public PersistenceUseType UseType { get; private set; }
        public DateTime ResetOn { get; private set; }

        public ReceiveMessageCache(IChangeStore changeStore, EndpointAddress address, PersistenceUseType useType)
            : base(changeStore)
        {
            Address = address;
            UseType = useType;
            Id = address + "|" + useType;
            
            this.messages = new ConcurrentDictionary<Guid, MessagePayload>();
            this.sequence = 1;           
        }

        public override void Initialise()
        {
            base.Initialise();

            GetMessages().ForEach(m =>
                Messenger.Send(new MessageLoadedToCache
                {
                    CacheAddress = Address,
                    UseType = UseType,
                    Message = m
                }));
        }

        public IEnumerable<MessagePayload> GetOrderedMessages()
        {
            return GetMessages().OrderBy(GetMessageSequence);
        }

        public IEnumerable<MessagePayload> GetMessages()
        {
            return this.messages.Values;
        }

        static int GetMessageSequence(MessagePayload message)
        {
            return message.HasSequence() ? message.GetSequence() : 0;
        }

        public void AddMessage(MessagePayload message)
        {
            AddChange(new AddMessageChange(message));

            Messenger.Send(new MessageAddedToCache
            {
                Message = message,
                CacheAddress = Address,
                UseType = UseType
            });
        }

        public void ApplyChange(AddMessageChange change)
        {
            if (!this.messages.TryAdd(change.Message.Id, change.Message))
            {
                this.messages[change.Message.Id] = change.Message;
            }
        }

        public int GetSequence()
        {
            return this.sequence;
        }

        public void Reset(int sequenceToResetTo, DateTime sequenceSetOn)
        {
            AddChange(new ResetCacheChange(sequenceToResetTo, sequenceSetOn));
        }

        public void ApplyChange(ResetCacheChange change)
        {
            this.sequence = change.Sequence;
            this.ResetOn = change.SetOn;
            this.messages.Clear();
        }

        public void Delete(Guid id)
        {
            AddChange(new DeleteMessageChange(id));

            Messenger.Send(new MessageRemovedFromCache
            {
                MessageId = id,
                Address = Address,
                UseType = UseType
            });
        }

        public void ApplyChange(DeleteMessageChange change)
        {
            ApplyDelete(change.Id);
        }

        public void DeleteAndSetSequence(Guid id, int toSet)
        {
            AddChange(new DeleteMessageAndSetSequenceChange(id, toSet));

            Messenger.Send(new MessageRemovedFromCache
            {
                MessageId = id,
                Address = Address,
                UseType = UseType
            });
        }

        public void ApplyChange(DeleteMessageAndSetSequenceChange change)
        {
            ApplyDelete(change.Id);
            this.sequence = change.Sequence;
        }

        void ApplyDelete(Guid id)
        {       
            MessagePayload temp;
            this.messages.TryRemove(id, out temp);
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