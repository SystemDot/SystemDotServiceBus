namespace SystemDot.Serialisation
{
    public interface ISerialiser 
    {
        byte[] Serialise(object toSerialise);
        object Deserialise(byte[] toDeserialise);
    }
}