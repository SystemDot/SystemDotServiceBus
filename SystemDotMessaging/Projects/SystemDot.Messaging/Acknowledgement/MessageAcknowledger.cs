using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Acknowledgement
{
    class MessageAcknowledger : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly AcknowledgementSender sender;

        public event Action<MessagePayload> MessageProcessed;

        public MessageAcknowledger(AcknowledgementSender sender)
        {
            Contract.Requires(sender != null);
            this.sender = sender;
        }

        public void InputMessage(MessagePayload toInput)
        {
            this.sender.SendAcknowledgement(toInput);
            this.MessageProcessed(toInput);
        }
    }
}