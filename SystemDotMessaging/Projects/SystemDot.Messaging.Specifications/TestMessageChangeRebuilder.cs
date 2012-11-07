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
        
        public TestMessageChangeRebuilder(IChangeStore store, EndpointAddress address, PersistenceUseType useType)
            : base(store)
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

        void ApplyChange(AddMessageAndIncrementSequenceChange change)
        {
            this.messages.Add(change.Message);
        }

        void ApplyChange(AddMessageChange change)
        {
            this.messages.Add(change.Message);
        }

        void ApplyChange(UpdateMessageChange change)
        {
        }

        void ApplyChange(SetSequenceChange change)
        {
        }

        void ApplyChange(DeleteMessageChange change)
        {
        }

        public EndpointAddress Address { get; private set; }
        public PersistenceUseType UseType { get; private set; }
    }
}