using System;
using System.Collections.Generic;
using System.IO;
using SystemDot.Core;
using SystemDot.Http;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.Http
{
    class MessageTransporter : IMessageTransporter
    {
        readonly IWebRequestor requestor;
        readonly ISerialiser formatter;

        public MessageTransporter(IWebRequestor requestor, ISerialiser formatter)
        {
            this.requestor = requestor;
            this.formatter = formatter;
        }

        public void TransportMessage(
            MessagePayload toTransport,
            Action<Exception> onException,
            Action onCompletion,
            Action<IEnumerable<MessagePayload>> onReceiveMessages)
        {
            requestor.SendPut(
                toTransport.GetToAddress().Server.GetUrl(),
                requestStream => formatter.Serialise(requestStream, toTransport),
                s => RecieveResponse(s, onReceiveMessages),
                onException,
                onCompletion);
        }

        public void TransportMessage(MessagePayload toTransport)
        {
            requestor.SendPut(
                toTransport.GetToAddress().Server.GetUrl(),
                requestStream => formatter.Serialise(requestStream, toTransport));
        }

        void RecieveResponse(Stream responseStream, Action<IEnumerable<MessagePayload>> onReceiveMessages)
        {
            onReceiveMessages(formatter.Deserialise(responseStream).As<IEnumerable<MessagePayload>>());
        }
    }
}