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

        public void Serialise(Stream toSerialise, object graph)
        {
            byte[] bytes = Serialise(graph);
            toSerialise.Write(bytes, 0, bytes.Length);
        }

        public object Deserialise(byte[] toDeserialise)
        {
            return SilverlightSerializer.Deserialize(toDeserialise);
        }

        public object Deserialise(Stream toDeserialise)
        {
            using (var ms = new MemoryStream())
            {
                toDeserialise.CopyTo(ms);
                return Deserialise(ms.ToArray());
            }
        }
    }
}