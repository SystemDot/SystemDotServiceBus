using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemDot.Messaging.Packaging;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Specifications.transport.http.remote.client
{
    public static class StreamEnumerableExtensions
    {
        public static IEnumerable<MessagePayload> DeserialiseToPayloads(this IEnumerable<Stream> streams)
        {
            return streams.Select(s => s.Deserialise<MessagePayload>(GetSerialiser()));
        }

        static ISerialiser GetSerialiser()
        {
            return new JsonSerialiser();
        }
    }
}