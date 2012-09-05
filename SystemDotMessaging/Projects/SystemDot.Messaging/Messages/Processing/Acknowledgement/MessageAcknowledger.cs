using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Messages.Processing.Acknowledgement
{
    public class MessageAcknowledger : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly IMessageSender sender;

        public event Action<MessagePayload> MessageProcessed;

        public MessageAcknowledger(IMessageSender sender)
        {
            Contract.Requires(sender != null);
            this.sender = sender;
        }

        public void InputMessage(MessagePayload toInput)
        {
            SendAcknowledgement(toInput);
            MessageProcessed(toInput);
        }

        void SendAcknowledgement(MessagePayload toInput)
        {
            var acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(toInput.Id);
            acknowledgement.SetToAddress(toInput.GetFromAddress());

            this.sender.InputMessage(acknowledgement);
        }
    }
}