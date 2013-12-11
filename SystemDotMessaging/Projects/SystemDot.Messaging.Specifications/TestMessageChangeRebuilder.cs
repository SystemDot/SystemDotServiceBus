using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Specifications
{
    public class TestMessageChangeRebuilder : ChangeRoot
    {
        readonly List<MessagePayload> messages;
        
        public TestMessageChangeRebuilder(
            ChangeStore changeStore, 
            EndpointAddress address, 
            PersistenceUseType useType, 
            ICheckpointStrategy checkPointStrategy)
            : base(changeStore, checkPointStrategy)
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

        public void ApplyChange(SetFirstItemCachedOnChange change)
        {
        }

        public void ApplyChange(AddMessageChange change)
        {
            if (this.messages.Contains(change.Message))
                this.messages.Remove(change.Message);

            this.messages.Add(change.Message);
        }

        public void ApplyChange(ResetCacheChange change)
        {
        }

        public void ApplyChange(DeleteMessageChange change)
        {
        }

        public void ApplyChange(DeleteMessageAndSetSequenceChange change)
        {
        }

        EndpointAddress Address { get; set; }
        PersistenceUseType UseType { get; set; }
        
        protected override void UrgeCheckPoint()
        {
        }
    }
}