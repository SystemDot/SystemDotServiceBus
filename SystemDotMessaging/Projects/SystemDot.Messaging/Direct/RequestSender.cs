using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Core.Collections;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Transport;
using SystemDot.ThreadMarshalling;

namespace SystemDot.Messaging.Direct
{
    class RequestSender : IMessageInputter<MessagePayload>
    {
        readonly IMessageTransporter messageTransporter;
        readonly MessageReceiver messageReceiver;
        readonly IMainThreadMarshaller marshaller;

        public RequestSender(IMessageTransporter messageTransporter, MessageReceiver messageReceiver, IMainThreadMarshaller marshaller)
        {
            Contract.Requires(messageTransporter != null);
            Contract.Requires(messageReceiver != null);

            this.messageTransporter = messageTransporter;
            this.messageReceiver = messageReceiver;
            this.marshaller = marshaller;
        }

        public void InputMessage(MessagePayload toInput)
        {
            toInput.SetIsDirectChannelMessage();
            SendPayloadToServer(toInput);
        }

        void SendPayloadToServer(MessagePayload toSend)
        {
            messageTransporter.TransportMessage(toSend, GetServerErrorAction(), () => { }, ReturnMessages);
        }

        Action<Exception> GetServerErrorAction()
        {
            return DirectSendContext.HasServerErrorAction() ? GetServerErrorActionOnMainThread() : e => { };
        }

        Action<Exception> GetServerErrorActionOnMainThread()
        {
            Action<Exception> serverErrorAction = DirectSendContext.GetServerErrorAction();
            return e => marshaller.RunOnMainThread(() => serverErrorAction(e));
        }

        void ReturnMessages(IEnumerable<MessagePayload> toReturn)
        {
            toReturn.ForEach(messageReceiver.InputMessage);
        }
    }
}