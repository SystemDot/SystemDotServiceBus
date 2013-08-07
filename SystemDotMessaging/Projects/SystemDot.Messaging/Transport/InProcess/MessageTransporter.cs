using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.InProcess
{
    class MessageTransporter : IMessageTransporter
    {
        readonly IInProcessMessageServer messageServer;
        readonly ISerialiser serialiser;

        public MessageTransporter(IInProcessMessageServer messageServer, ISerialiser serialiser)
        {
            Contract.Requires(messageServer != null);
            Contract.Requires(serialiser != null);

            this.messageServer = messageServer;
            this.serialiser = serialiser;
        }

        public void TransportMessage(
            MessagePayload toTransport,
            Action<Exception> onException,
            Action onCompletion,
            Action<IEnumerable<MessagePayload>> onReceiveMessages)
        {
            try
            {
                onReceiveMessages(messageServer.InputMessage(CopyMessage(toTransport)));
                onCompletion();
            }
            catch (Exception e)
            {
                onException(e);
                throw;
            }
        }

        MessagePayload CopyMessage(MessagePayload toCopy)
        {
            return serialiser.Copy(toCopy);
        }

        public void TransportMessage(MessagePayload toTransport)
        {
            messageServer.InputMessage(CopyMessage(toTransport));
        }
    }
}