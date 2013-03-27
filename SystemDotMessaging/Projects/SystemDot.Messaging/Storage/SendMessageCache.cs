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
        readonly ISystemTime systemTime;
        readonly ConcurrentDictionary<Guid, MessagePayload> messages;
        int sequence;

        public EndpointAddress Address { get; private set; }
        public PersistenceUseType UseType { get; private set; }
        public DateTime FirstItemCachedOn { get; private set; }

        public SendMessageCache(
            ISystemTime systemTime, 
            IChangeStore changeStore, 
            EndpointAddress address, 
            PersistenceUseType useType)
            : base(changeStore)
        {
            Contract.Requires(systemTime != null);
            Contract.Requires(changeStore != null);
            Contract.Requires(address != EndpointAddress.Empty);

            this.systemTime = systemTime;
            Address = address;
            UseType = useType;
            Id = address + "|" + useType;

            this.messages = new ConcurrentDictionary<Guid, MessagePayload>();
            this.sequence = 1;
        }

        public IEnumerable<MessagePayload> GetMessages()
        {
            return this.messages.Values.OrderBy(GetMessageSequence);
        }

        static int GetMessageSequence(MessagePayload message)
        {
            return message.HasSequence() ? message.GetSequence() : 0;
        }

        public void AddMessageAndIncrementSequence(MessagePayload message)
        {
            if(FirstItemCachedOn == DateTime.MinValue)
                AddChange(new SetFirstItemCachedOnChange(this.systemTime.GetCurrentDate()));

            AddChange(new AddMessageAndIncrementSequenceChange(message, this.sequence + 1));

            Messenger.Send(new MessageAddedToCache { CacheAddress = Address, Message = message });
        }

        public void ApplyChange(SetFirstItemCachedOnChange change)
        {
            FirstItemCachedOn = change.On;
        }

        public void ApplyChange(AddMessageAndIncrementSequenceChange change)
        {
            this.sequence = change.Sequence;
            this.messages.TryAdd(change.Message.Id, change.Message);
        }

        public void UpdateMessage(MessagePayload message)
        {
            MessagePayload temp;
            if (this.messages.TryGetValue(message.Id, out temp)) this.messages[message.Id] = message;
        }

        public int GetSequence()
        {
            return this.sequence;
        }

        public void Delete(Guid id)
        {
            AddChange(new DeleteMessageChange(id));
        }

        public void ApplyChange(DeleteMessageChange change)
        {
            MessagePayload temp;
            this.messages.TryRemove(change.Id, out temp);
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

        public int GetFirstSequenceInCache()
        {
            return GetMessages().Any()
                ? GetMessages().First().GetSequence()
                : 0;
        }
    }
}