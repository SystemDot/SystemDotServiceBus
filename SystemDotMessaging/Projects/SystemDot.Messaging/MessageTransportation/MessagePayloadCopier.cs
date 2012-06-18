using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SystemDot.Messaging.MessageTransportation
{
    public class MessagePayloadCopier
    {
        public MessagePayload Copy(MessagePayload toCopy)
        {
            Contract.Requires(toCopy != null);
            
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, toCopy);
                stream.Seek(0, 0);
                return new BinaryFormatter().Deserialize(stream).As<MessagePayload>();
            }
        }
    }
}