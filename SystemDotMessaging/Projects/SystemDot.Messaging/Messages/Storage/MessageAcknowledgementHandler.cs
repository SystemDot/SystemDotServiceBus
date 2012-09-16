using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Processing.Acknowledgement;

namespace SystemDot.Messaging.Messages.Storage
{
    public class MessageAcknowledgementHandler : IMessageInputter<MessagePayload>
    {
        readonly IMessageStore messageStore;

        public MessageAcknowledgementHandler(IMessageStore messageStore)
        {
            Contract.Requires(messageStore != null);
            this.messageStore = messageStore;
        }

        public void InputMessage(MessagePayload toInput)
        {
            if (!toInput.IsAcknowledgement()) return;
            
            this.messageStore.Remove(toInput.GetAcknowledgementId());
        }
    }
}