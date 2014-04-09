using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using SystemDot.Core;
using SystemDot.Http;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.Http
{
    class HttpMessagingServer : IHttpHandler 
    {
        readonly ISerialiser serialiser;
        readonly IMessagingServerHandler[] handlers;

        public HttpMessagingServer(ISerialiser serialiser, params IMessagingServerHandler[] handlers)
        {
            Contract.Requires(serialiser != null);
            
            this.serialiser = serialiser;
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

            this.serialiser.Serialise(outputStream, outgoingMessages);
        }

        object DeserialiseMessage(Stream inputStream)
        {
            try
            {
                return this.serialiser.Deserialise(inputStream);
            }
            catch (CannotDeserialiseException e)
            {
                Logger.Error(e);
                return null;
            }
        }
    }
}