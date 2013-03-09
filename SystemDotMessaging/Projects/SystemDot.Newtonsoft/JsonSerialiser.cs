using System.IO;
using SystemDot.Serialisation;
using Newtonsoft.Json;

namespace SystemDot.Newtonsoft
{
    public class JsonSerialiser : ISerialiser
    {
        readonly JsonSerializer inner;

        public JsonSerialiser()
        {
            this.inner = new JsonSerializer();
        }

        public byte[] Serialise(object toSerialise)
        {
            using (var stream = new MemoryStream())
            {
                Serialise(stream, toSerialise);
                return stream.ToArray();
            }
        }

        public void Serialise(Stream toSerialise, object graph)
        {
            using (TextWriter streamWriter = new StreamWriter(toSerialise))
                using(JsonWriter textWriter = new JsonTextWriter(streamWriter))
                    this.inner.Serialize(textWriter, toSerialise);
        }

        public object Deserialise(byte[] toDeserialise)
        {
            using (var stream = new MemoryStream(toDeserialise))
            {
                return Deserialise(stream);
            }
        }

        public object Deserialise(Stream toDeserialise)
        {
            using (TextReader streamReader = new StreamReader(toDeserialise))
                using (JsonReader reader = new JsonTextReader(streamReader))
                    return this.inner.Deserialize(reader);
        }
    }
}
