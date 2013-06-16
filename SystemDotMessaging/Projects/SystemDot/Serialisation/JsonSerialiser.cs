using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SystemDot.Serialisation
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
            var stringBuilder = new StringBuilder();

            using (var stringWriter = new StringWriter(stringBuilder))
                using (var textWriter = new JsonTextWriter(stringWriter))
                    this.typedSerializer.Serialize(textWriter, toSerialise);

            return stringBuilder.ToString().ToBytes();
        }

        public void Serialise(Stream toSerialise, object graph)
        {
            byte[] bytes = Serialise(graph);
            toSerialise.Write(bytes, 0, bytes.Length);
        }

        public object Deserialise(byte[] toDeserialise)
        {
            using (var stringReader = new StringReader(toDeserialise.GetString()))
                using (var reader = new JsonTextReader(stringReader))
                    return this.typedSerializer.Deserialize(reader);
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
