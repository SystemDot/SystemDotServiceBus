using System.IO;

namespace SystemDot.Serialisation
{
    public interface ISerialiser 
    {
        byte[] Serialise(object toSerialise);
        void Serialize(Stream toSerialise, object graph);
        object Deserialise(byte[] toDeserialise);
        object Deserialize(Stream toDeserialise);
    }
}