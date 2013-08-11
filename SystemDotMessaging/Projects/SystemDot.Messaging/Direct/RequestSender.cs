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
        readonly Action<Exception> toRunOnException;

        public RequestSender(IMessageTransporter messageTransporter, MessageReceiver messageReceiver, Action<Exception> toRunOnException)
        {
            Contract.Requires(messageTransporter != null);
            Contract.Requires(messageReceiver != null);
            Contract.Requires(toRunOnException != null);

            this.messageTransporter = messageTransporter;
            this.messageReceiver = messageReceiver;
            this.toRunOnException = toRunOnException;
        }

        public void InputMessage(MessagePayload toInput)
        {
            toInput.SetIsDirectChannelMessage();
            SendPayloadToServer(toInput);
        }

        void SendPayloadToServer(MessagePayload toSend)
        {
            messageTransporter.TransportMessage(toSend, toRunOnException, () => { }, ReturnMessages);
        }

        void ReturnMessages(IEnumerable<MessagePayload> toReturn) 
        {
            toReturn.ForEach(messageReceiver.InputMessage);
        }
    }
}