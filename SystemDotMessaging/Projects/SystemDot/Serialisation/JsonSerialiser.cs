using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SystemDot.Serialisation
{
    public class JsonSerialiser : ISerialiser
    {
        readonly JsonSerializer serialiser;

        public JsonSerialiser()
        {
            this.serialiser = new JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.Auto,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public byte[] Serialise(object toSerialise)
        {
            Contract.Requires(toSerialise != null);
            
            using (var stream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(stream, Encoding.UTF8))
                {
                    this.serialiser.Serialize(new JsonTextWriter(streamWriter), toSerialise);
                }
                return stream.GetBuffer();
            }
        }

        public object Deserialise(byte[] toDeserialise)
        {
            Contract.Requires(toDeserialise != null);

            using (var stream = new MemoryStream(toDeserialise))
                using (var reader = new StreamReader(stream))
                    return this.serialiser.Deserialize<object>(new JsonTextReader(reader));
        }
    }
}