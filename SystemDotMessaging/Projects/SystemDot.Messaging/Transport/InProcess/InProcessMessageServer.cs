using SystemDot.Messaging.Packaging;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.InProcess
{
    class InProcessMessageServer : MessageProcessor, IInProcessMessageServer
    {
        readonly ISerialiser serialiser;

        public InProcessMessageServer(ISerialiser serialiser)
        {
            this.serialiser = serialiser;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            OnMessageProcessed(this.serialiser.Copy(toInput));
        }
    }
}