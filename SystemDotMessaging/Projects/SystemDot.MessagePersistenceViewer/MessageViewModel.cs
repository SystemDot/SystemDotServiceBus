using System;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.Sequencing;

namespace SystemDot.MessagePersistenceViewer
{
    public class MessageViewModel
    {
        public MessagePayload Message { get; set; }

        public Guid Id { get; private set; }

        public int Sequence { get; private set; }

        public int AmountSent { get; private set; }

        public bool IsDeleted { get; set; }

        public bool UpdatedWithoutAdd { get; set; }

        public MessageViewModel(MessagePayload toAdd)
        {
            this.Id = toAdd.Id;
            this.Sequence = toAdd.GetSequence();
            this.AmountSent = toAdd.GetAmountSent();
            this.Message = toAdd;
        }
    }
}