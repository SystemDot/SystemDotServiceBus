using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.Errors
{
    public class ErrorReciever : MessagePayloadCopier
    {
        public ErrorReciever(ISerialiser serialiser) : base(serialiser)
        {
        }
    }
}