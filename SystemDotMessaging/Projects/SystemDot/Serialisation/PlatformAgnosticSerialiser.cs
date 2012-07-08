using System.IO;
using Serialization;

namespace SystemDot.Serialisation
{
    public class PlatformAgnosticSerialiser : ISerialiser
    {
        public byte[] Serialise(object toSerialise)
        {
            return SilverlightSerializer.Serialize(toSerialise);
        }

        public void Serialize(Stream toSerialise, object graph)
        {
            using (var ms = new MemoryStream(Serialise(graph))) ms.CopyTo(toSerialise);
        }

        public object Deserialise(byte[] toDeserialise)
        {
            return SilverlightSerializer.Deserialize(toDeserialise);
        }

        public object Deserialize(Stream toDeserialise)
        {
            using (var ms = new MemoryStream())
            {
                toDeserialise.CopyTo(ms);
                return Deserialise(ms.ToArray());
            }
        }
    }
}