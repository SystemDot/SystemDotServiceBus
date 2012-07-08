using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization;
using SystemDot.Http;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.Http.LongPolling.Servers
{
    public class HttpMessagingServer : IHttpHandler 
    {
        private readonly ISerialiser formatter;
        private readonly IMessagingServerHandler[] handlers;

        public HttpMessagingServer(ISerialiser formatter, params IMessagingServerHandler[] handlers)
        {
            Contract.Requires(formatter != null);
            
            this.formatter = formatter;
            this.handlers = handlers;
        }

        public void HandleRequest(Stream inputStream, Stream outputStream)
        {
            object deserialised = this.formatter.Deserialize(inputStream);

            if (!(deserialised is MessagePayload)) return;

            var message = deserialised.As<MessagePayload>();
            var outgoingMessages = new List<MessagePayload>();
             
            foreach (IMessagingServerHandler handler in this.handlers)
                handler.HandleMessage(message, outgoingMessages);

            this.formatter.Serialize(outputStream, outgoingMessages);
        }
    }
}