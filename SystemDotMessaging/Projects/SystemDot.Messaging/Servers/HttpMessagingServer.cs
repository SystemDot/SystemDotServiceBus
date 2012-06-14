using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization;
using SystemDot.Http;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Servers
{
    public class HttpMessagingServer : IHttpHandler 
    {
        private readonly IFormatter formatter;
        private readonly IMessagingServerHandler[] handlers;

        public HttpMessagingServer(IFormatter formatter, params IMessagingServerHandler[] handlers)
        {
            Contract.Requires(formatter != null);
            
            this.formatter = formatter;
            this.handlers = handlers;
        }

        public void HandleRequest(Stream inputStream, Stream outputStream)
        {
            MessagePayload message = DeserialiseMessage(inputStream);

            foreach (IMessagingServerHandler handler in this.handlers)
            {
                if (handler.ShouldHandleMessage(message)) handler.HandleMessage(message);

                this.formatter.Serialize(outputStream, handler.Reply());
            }
        }

        private MessagePayload DeserialiseMessage(Stream inputStream)
        {
            return (MessagePayload) this.formatter.Deserialize(inputStream);
        }
    }
}