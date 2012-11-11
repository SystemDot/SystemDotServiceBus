using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Acknowledgement
{
    public class MessageAcknowledgementHandler : IMessageInputter<MessagePayload>
    {
        private readonly List<IPersistence> persistences;

        public MessageAcknowledgementHandler()
        {
            this.persistences = new List<IPersistence>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            if (!toInput.IsAcknowledgement()) return;

            var id = toInput.GetAcknowledgementId();

            this.persistences
                .Single(p => p.UseType == id.UseType && p.Address == id.Address)
                .Delete(id.MessageId);
        }

        public void RegisterPersistence(IPersistence toRegister)
        {
            Contract.Requires(toRegister != null);
            this.persistences.Add(toRegister);
        }
    }
}