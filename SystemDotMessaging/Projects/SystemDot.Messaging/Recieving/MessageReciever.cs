using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization;
using SystemDot.Http;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Pipes;

namespace SystemDot.Messaging.Recieving
{
    public class MessageReciever : IHttpHandler
    {
        readonly IPipe<MessagePayload> pipe;
        readonly IFormatter serialiser;

        public MessageReciever(IPipe<MessagePayload> pipe, IFormatter serialiser)
        {
            Contract.Requires(pipe != null);
            Contract.Requires(serialiser != null);
            
            this.pipe = pipe;
            this.serialiser = serialiser;
        }

        public void HandleRequest(Stream inputStream)
        {
            this.pipe.Push(DeserializePayload(inputStream));
        }

        private MessagePayload DeserializePayload(Stream inputStream)
        {
            return (MessagePayload)this.serialiser.Deserialize(inputStream);
        }

        public void Respond(Stream outputStream)
        {
        }
    }
}