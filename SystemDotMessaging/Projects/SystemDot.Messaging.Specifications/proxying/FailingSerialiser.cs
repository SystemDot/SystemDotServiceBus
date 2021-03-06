using System;
using System.IO;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Specifications.proxying
{
    public class FailingSerialiser : ISerialiser
    {
        public byte[] Serialise(object toSerialise)
        {
            throw new Exception();
        }

        public string SerialiseToString(object toSerialise)
        {
            throw new Exception();
        }

        public void Serialise(Stream toSerialise, object graph)
        {
            throw new Exception();
        }

        public object Deserialise(byte[] toDeserialise)
        {
            throw new CannotDeserialiseException(new Exception());
        }

        public object Deserialise(Stream toDeserialise)
        {
            return Deserialise(new byte[0]);
        }

        public string DeserialiseToString(byte[] toDeserialise)
        {
            throw new CannotDeserialiseException(new Exception());
        }
    }
}