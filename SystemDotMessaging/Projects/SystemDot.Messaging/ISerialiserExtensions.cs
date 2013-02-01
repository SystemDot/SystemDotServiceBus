using SystemDot.Messaging.Packaging;
using SystemDot.Serialisation;

namespace SystemDot.Messaging
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