using System.IO;

namespace SystemDot.Serialisation
{
    public interface ISerialiser
    {
        byte[] Serialise(object toSerialise);
        void Serialise(Stream toSerialise, object graph);
        object Deserialise(byte[] toDeserialise);
        object Deserialise(Stream toDeserialise);
    }
}