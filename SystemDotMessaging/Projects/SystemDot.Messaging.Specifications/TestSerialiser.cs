using System.IO;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Specifications
{
    public class TestSerialiser : ISerialiser
    {
        public object Graph { get; private set; }

        public byte[] Serialise(object toSerialise)
        {
            throw new System.NotImplementedException();
        }

        public void Serialise(Stream toSerialise, object graph)
        {
            this.Graph = graph;
        }

        public object Deserialise(byte[] toDeserialise)
        {
            throw new System.NotImplementedException();
        }

        public object Deserialise(Stream toDeserialise)
        {
            throw new System.NotImplementedException();
        }
    }
}