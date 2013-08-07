using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Direct
{
    class DirectChannelMessageSender : IMessageInputter<MessagePayload>
    {
        readonly IMessageTransporter messageTransporter;

        public DirectChannelMessageSender(IMessageTransporter messageTransporter)
        {
            Contract.Requires(messageTransporter != null);

            this.messageTransporter = messageTransporter;
        }

        public void InputMessage(MessagePayload toInput)
        {
            SendPayloadToServer(toInput);
        }

        void SendPayloadToServer(MessagePayload toSend)
        {
            messageTransporter.TransportMessage(toSend);
        }
    }
}