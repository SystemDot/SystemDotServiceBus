using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.Acknowledgement
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
            acknowledgement.SetAcknowledgementId(toInput.GetSourcePersistenceId());
            acknowledgement.SetToAddress(toInput.GetFromAddress());

            this.sender.InputMessage(acknowledgement);
        }
    }
}