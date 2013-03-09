using System.IO;
using System.Text;
using SystemDot.Serialisation;
using Newtonsoft.Json;

namespace SystemDot.Newtonsoft
{
    public class JsonSerialiser : ISerialiser
    {
        readonly JsonSerializer typedSerializer;

        public JsonSerialiser()
        {
            this.typedSerializer = new JsonSerializer
		    {
			    TypeNameHandling = TypeNameHandling.All,
			    DefaultValueHandling = DefaultValueHandling.Ignore,
			    NullValueHandling = NullValueHandling.Ignore
		    };
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
            using (var streamWriter = new StreamWriter(toSerialise, Encoding.UTF8))
                using (var textWriter = new JsonTextWriter(streamWriter))
                    this.typedSerializer.Serialize(textWriter, graph);
        }

        public object Deserialise(byte[] toDeserialise)
        {
            using (var stream = new MemoryStream(toDeserialise))
                return Deserialise(stream);
        }

        public object Deserialise(Stream toDeserialise)
        {
            using (var streamReader = new StreamReader(toDeserialise, Encoding.UTF8))
                using (JsonReader reader = new JsonTextReader(streamReader))
                    return this.typedSerializer.Deserialize(reader);
        }
    }
}
