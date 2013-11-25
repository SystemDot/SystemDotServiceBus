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
    public class SendMessageCache : ChangeRoot, IMessageCache
    {
        readonly ConcurrentDictionary<Guid, MessagePayload> messages;
        readonly ISystemTime systemTime;
        volatile int sequence;

        public SendMessageCache(ISystemTime systemTime, ChangeStore changeStore, EndpointAddress address, PersistenceUseType useType)
            : base(changeStore)
        {
            Contract.Requires(systemTime != null);
            Contract.Requires(changeStore != null);
            Contract.Requires(address != EndpointAddress.Empty);

            this.systemTime = systemTime;
            Address = address;
            UseType = useType;
            Id = address + "|" + useType;

            messages = new ConcurrentDictionary<Guid, MessagePayload>();
            sequence = 1;
        }

        public DateTime FirstItemCachedOn { get; private set; }

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

        void NotifyMessageLoadedToCache(MessagePayload m)
        {
            Messenger.Send(new MessageLoadedToCache
            {
                CacheAddress = Address,
                UseType = UseType,
                Message = m
            });
        }

        static int GetMessageSequence(MessagePayload message)
        {
            return message.HasSequence() ? message.GetSequence() : 0;
        }

        public void AddMessageAndIncrementSequence(MessagePayload message)
        {
            if (FirstItemCachedOn == DateTime.MinValue) AddChange(new SetFirstItemCachedOnChange(systemTime.GetCurrentDate()));

            AddChange(new AddMessageAndIncrementSequenceChange(message, sequence + 1));

            NotifyMessageAddedToCache(message);
        }

        void NotifyMessageAddedToCache(MessagePayload message)
        {
            Messenger.Send(new MessageAddedToCache
            {
                CacheAddress = Address,
                UseType = UseType,
                Message = message
            });
        }

        public void ApplyChange(SetFirstItemCachedOnChange change)
        {
            FirstItemCachedOn = change.On;
        }

        public void ApplyChange(AddMessageAndIncrementSequenceChange change)
        {
            sequence = change.Sequence;
            messages.TryAdd(change.Message.Id, change.Message);
        }

        public void UpdateMessage(MessagePayload message)
        {
            messages.UpdateIfExists(message.Id, message);
        }

        public int GetSequence()
        {
            return sequence;
        }

        public void ApplyChange(DeleteMessageChange change)
        {
            messages.Remove(change.Id);
        }

        protected override void UrgeCheckPoint()
        {
            if (messages.Count == 0) CheckPoint(CreateCheckpoint());
        }

        MessageCheckpointChange CreateCheckpoint()
        {
            return new MessageCheckpointChange
            {
                Sequence = sequence,
                CachedOn = FirstItemCachedOn
            };
        }

        public void ApplyChange(MessageCheckpointChange change)
        {
            sequence = change.Sequence;
            FirstItemCachedOn = change.CachedOn;
        }

        public int GetFirstSequenceInCache()
        {
            return GetMessages().Any() ? GetMessages().Min(m => m.GetSequence()) : 0;
        }
    }
}