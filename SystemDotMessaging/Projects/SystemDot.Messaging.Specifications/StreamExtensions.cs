using System.IO;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Specifications
{
    public static class StreamExtensions
    {
        public static T Deserialise<T>(this Stream stream, ISerialiser formatter)
        {
            stream.Seek(0, 0);
            return formatter.Deserialise(stream).As<T>();
        }

        public static void Serialise(this Stream stream, object toSerialise, ISerialiser formatter)
        {
            formatter.Serialise(stream, toSerialise);
            stream.Seek(0, 0);
        }
    }
}