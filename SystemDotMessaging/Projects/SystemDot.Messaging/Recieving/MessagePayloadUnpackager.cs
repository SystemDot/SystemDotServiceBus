using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using SystemDot.Pipes;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Recieving
{
    public class MessagePayloadUnpackager
    {
        readonly IPipe<object> outputPipe;
        private readonly ISerialiser serialiser;

        public MessagePayloadUnpackager(IPipe<MessagePayload> inputPipe, IPipe<object> outputPipe, ISerialiser serialiser)
        {
            Contract.Requires(inputPipe != null);
            Contract.Requires(outputPipe != null);
            Contract.Requires(serialiser != null);

            inputPipe.ItemPushed += OnInputPipeItemPushed;
            this.outputPipe = outputPipe;
            this.serialiser = serialiser;
        }

        void OnInputPipeItemPushed(MessagePayload payload)
        {
            this.outputPipe.Push(this.serialiser.Deserialise(payload.GetBody()));
        }
    }
}