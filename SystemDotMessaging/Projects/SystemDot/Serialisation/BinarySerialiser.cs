using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SystemDot.Serialisation
{
    public class BinarySerialiser : ISerialiser
    {
        readonly IFormatter formatter;

        public BinarySerialiser(IFormatter formatter)
        {
            this.formatter = formatter;
        }

        public byte[] Serialise(object toSerialise)
        {
            Contract.Requires(toSerialise != null);
            
            using (var stream = new MemoryStream())
            {
                this.formatter.Serialize(stream, toSerialise);
                return stream.GetBuffer();
            }
        }

        public object Deserialise(byte[] toDeserialise)
        {
            Contract.Requires(toDeserialise != null);

            using (var stream = new MemoryStream(toDeserialise))
                 return this.formatter.Deserialize(stream);
        }
    }
}