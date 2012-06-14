using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using SystemDot.Pipes;

namespace SystemDot.Messaging.Recieving
{
    public class MessagePayloadUnpackager
    {
        readonly IPipe<object> outputPipe;

        public MessagePayloadUnpackager(IPipe<MessagePayload> inputPipe, IPipe<object> outputPipe)
        {
            Contract.Requires(inputPipe != null);
            Contract.Requires(outputPipe != null);

            inputPipe.ItemPushed += OnInputPipeItemPushed;
            this.outputPipe = outputPipe;
        }

        void OnInputPipeItemPushed(MessagePayload payload)
        {
            this.outputPipe.Push(payload.GetBody());
        }
    }
}