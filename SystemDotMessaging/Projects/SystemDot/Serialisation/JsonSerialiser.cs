using System;
using System.IO;
using System.Text;
using SystemDot.Logging;
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
            return SerialiseToString(toSerialise).ToBytes();
        }

        public string SerialiseToString(object toSerialise)
        {
            var stringBuilder = new StringBuilder();

            using (var stringWriter = new StringWriter(stringBuilder))
            using (var textWriter = new JsonTextWriter(stringWriter))
                this.typedSerializer.Serialize(textWriter, toSerialise);

            return stringBuilder.ToString();
        }

        public void Serialise(Stream toSerialise, object graph)
        {
            byte[] bytes = Serialise(graph);
            toSerialise.Write(bytes, 0, bytes.Length);
        }

        public object Deserialise(byte[] toDeserialise)
        {
            try
            {
                return DeserialiseFromUtf8(toDeserialise);
            }
            catch
            {
                return DeserialiseFromNoEncoding(toDeserialise);
            }
        }

        object DeserialiseFromUtf8(byte[] toDeserialise)
        {
            return DeserialiseFromString(toDeserialise.GetStringFromUtf8());
        }

        object DeserialiseFromNoEncoding(byte[] toDeserialise)
        {
            try
            {
                return DeserialiseFromString(toDeserialise.GetStringFromNoEncoding());
            }
            catch (Exception e)
            {
                throw new CannotDeserialiseException(e);
            }
        }

        object DeserialiseFromString(string toDeserialise)
        {
            using (var stringReader = new StringReader(toDeserialise))
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
