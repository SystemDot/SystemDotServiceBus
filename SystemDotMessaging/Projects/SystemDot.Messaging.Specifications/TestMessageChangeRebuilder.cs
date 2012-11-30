using System.Collections.Generic;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Specifications
{
    public class TestMessageChangeRebuilder : ChangeRoot
    {
        readonly List<MessagePayload> messages;
        
        public TestMessageChangeRebuilder(IChangeStore changeStore, EndpointAddress address, PersistenceUseType useType)
            : base(changeStore)
        {
            Address = address;
            UseType = useType;
            Id = address + "|" + useType;

            this.messages = new List<MessagePayload>();
        }

        public IEnumerable<MessagePayload> GetMessages()
        {
            return this.messages;
        }

        public void ApplyChange(AddMessageAndIncrementSequenceChange change)
        {
            this.messages.Add(change.Message);
        }

        public void ApplyChange(AddMessageChange change)
        {
            this.messages.Add(change.Message);
        }

        public void ApplyChange(UpdateMessageChange change)
        {
        }

        public void ApplyChange(SetSequenceChange change)
        {
        }

        public void ApplyChange(DeleteMessageChange change)
        {
        }

        public void ApplyChange(DeleteMessageAndSetSequenceChange change)
        {
        }

        public EndpointAddress Address { get; private set; }
        public PersistenceUseType UseType { get; private set; }
        
        protected override void UrgeCheckPoint()
        {
        }
    }
}