using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.Pipes;
using SystemDot.Messaging.Servers;
using SystemDot.Threading;

namespace SystemDot.Messaging.Recieving
{
    public class MessageReciever : IHttpHandler
    {
        readonly IPipe pipe;
        
        public MessageReciever(IPipe pipe)
        {
            Contract.Requires(pipe != null);
            
            this.pipe = pipe;
        }

        public void HandleRequest(Stream inputStream)
        {
            this.pipe.Publish(DeserialiseMessage(inputStream));
        }

        public void Respond(Stream outputStream)
        {
        }

        static object DeserialiseMessage(Stream toDeserialise)
        {
            return new BinaryFormatter().Deserialize(toDeserialise);
        }
    }
}