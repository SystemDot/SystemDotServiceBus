using System.Diagnostics.Contracts;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.Packaging
{
    public class MessagePayloadCopier
    {
        readonly ISerialiser serialiser;

        public MessagePayloadCopier(ISerialiser serialiser)
        {
            Contract.Requires(serialiser != null);
            this.serialiser = serialiser;
        }

        public MessagePayload Copy(MessagePayload toCopy)
        {
            Contract.Requires(toCopy != null);

            return serialiser.Deserialise(serialiser.Serialise(toCopy)).As<MessagePayload>();
        }
    }
}