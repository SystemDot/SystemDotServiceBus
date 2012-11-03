using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels
{
    public static class ISerialiserExtensions
    {
        public static MessagePayload Copy(this ISerialiser serialiser, MessagePayload toCopy)
        {
            return serialiser
                .Deserialise(serialiser.Serialise(toCopy))
                .As<MessagePayload>();
        }
    }
}