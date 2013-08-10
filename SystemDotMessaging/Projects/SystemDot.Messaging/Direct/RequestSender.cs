using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Direct
{
    class RequestSender : IMessageInputter<MessagePayload>
    {
        readonly IMessageTransporter messageTransporter;
        readonly MessageReceiver messageReceiver;

        public RequestSender(IMessageTransporter messageTransporter, MessageReceiver messageReceiver)
        {
            Contract.Requires(messageTransporter != null);

            this.messageTransporter = messageTransporter;
            this.messageReceiver = messageReceiver;
        }

        public void InputMessage(MessagePayload toInput)
        {
            toInput.SetIsDirectChannelMessage();
            SendPayloadToServer(toInput);
        }

        void SendPayloadToServer(MessagePayload toSend)
        {
            messageTransporter.TransportMessage(toSend, exception => { }, () => { }, ReturnMessages);
        }

        void ReturnMessages(IEnumerable<MessagePayload> toReturn) 
        {
            toReturn.ForEach(messageReceiver.InputMessage);
        }
    }
}