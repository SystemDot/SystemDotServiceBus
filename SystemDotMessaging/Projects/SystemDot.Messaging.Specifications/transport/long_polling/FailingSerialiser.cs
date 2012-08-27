using System;
using System.IO;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Specifications.transport.long_polling
{
    public class FailingSerialiser : ISerialiser
    {
        public byte[] Serialise(object toSerialise)
        {
            throw new Exception();
        }

        public void Serialise(Stream toSerialise, object graph)
        {
            throw new Exception();
        }

        public object Deserialise(byte[] toDeserialise)
        {
            throw new Exception();
        }

        public object Deserialise(Stream toDeserialise)
        {
            throw new Exception();
        }
    }
}