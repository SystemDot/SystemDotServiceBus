using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using SystemDot.Http;
using SystemDot.Messaging.Packaging;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.Http
{
    class HttpMessagingServer : IHttpHandler 
    {
        readonly ISerialiser formatter;
        readonly IMessagingServerHandler[] handlers;

        public HttpMessagingServer(ISerialiser formatter, params IMessagingServerHandler[] handlers)
        {
            Contract.Requires(formatter != null);
            
            this.formatter = formatter;
            this.handlers = handlers;
        }

        public void HandleRequest(Stream inputStream, Stream outputStream)
        {
            object deserialised = DeserialiseMessage(inputStream);

            if (deserialised == null) return;
            if (!(deserialised is MessagePayload)) return;

            var message = deserialised.As<MessagePayload>();
            var outgoingMessages = new List<MessagePayload>();
             
            foreach (IMessagingServerHandler handler in this.handlers)
                handler.HandleMessage(message, outgoingMessages);

            this.formatter.Serialise(outputStream, outgoingMessages);
        }

        object DeserialiseMessage(Stream inputStream)
        {
            try
            {
                return this.formatter.Deserialise(inputStream);
            }
            catch (CannotDeserialiseException)
            {
                return null;
            }
        }
    }
}