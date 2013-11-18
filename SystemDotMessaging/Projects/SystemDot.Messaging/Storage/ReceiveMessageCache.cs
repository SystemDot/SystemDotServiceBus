using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
        readonly ConcurrentDictionary<Guid, MessagePayload> messages;
        volatile int sequence;

        public ReceiveMessageCache(IChangeStore changeStore, EndpointAddress address, PersistenceUseType useType)
            : base(changeStore)
        {
            Contract.Requires(changeStore != null);
            Contract.Requires(address != EndpointAddress.Empty);
            
            Address = address;
            UseType = useType;
            Id = address + "|" + useType;

            messages = new ConcurrentDictionary<Guid, MessagePayload>();
            sequence = 1;
        }

        public DateTime ResetOn { get; private set; }

        public EndpointAddress Address { get; private set; }

        public PersistenceUseType UseType { get; private set; }

        public IEnumerable<MessagePayload> GetOrderedMessages()
        {
            return GetMessages().OrderBy(GetMessageSequence);
        }

        public IEnumerable<MessagePayload> GetMessages()
        {
            return messages.Values;
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

        public override void Initialise()
        {
            base.Initialise();
            GetMessages().ForEach(NotifyMessageLoadedToCache);
        }

        void NotifyMessageLoadedToCache(MessagePayload message)
        {
            Messenger.Send(new MessageLoadedToCache
            {
                CacheAddress = Address,
                UseType = UseType,
                Message = message
            });
        }

        static int GetMessageSequence(MessagePayload message)
        {
            return message.HasSequence() ? message.GetSequence() : 0;
        }

        public void AddMessage(MessagePayload message)
        {
            AddChange(new AddMessageChange(message));
            NotifyMessageAddedToCache(message);
        }

        void NotifyMessageAddedToCache(MessagePayload message)
        {
            Messenger.Send(new MessageAddedToCache
            {
                Message = message,
                CacheAddress = Address,
                UseType = UseType
            });
        }

        public void ApplyChange(AddMessageChange change)
        {
            messages.AddOrUpdate(change.Message.Id, change.Message);
        }

        public int GetSequence()
        {
            return sequence;
        }

        public void Reset(int sequenceToResetTo, DateTime sequenceSetOn)
        {
            AddChange(new ResetCacheChange(sequenceToResetTo, sequenceSetOn));
        }

        public void ApplyChange(ResetCacheChange change)
        {
            sequence = change.Sequence;
            ResetOn = change.SetOn;
            messages.Clear();
        }

        public void ApplyChange(DeleteMessageChange change)
        {
            ApplyDelete(change.Id);
        }

        public void DeleteAndSetSequence(Guid id, int toSet)
        {
            AddChange(new DeleteMessageAndSetSequenceChange(id, toSet));
            NotifyMessageRemovedFromCache(id);
        }

        void NotifyMessageRemovedFromCache(Guid id)
        {
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
            sequence = change.Sequence;
        }

        void ApplyDelete(Guid id)
        {
            messages.Remove(id);
        }

        protected override void UrgeCheckPoint()
        {
            if (messages.Count == 0) CheckPoint(new MessageCheckpointChange { Sequence = sequence });
        }

        public void ApplyChange(MessageCheckpointChange change)
        {
            sequence = change.Sequence;
        }
    }
}