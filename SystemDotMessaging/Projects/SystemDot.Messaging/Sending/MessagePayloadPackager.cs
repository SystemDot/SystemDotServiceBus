using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using SystemDot.Pipes;

namespace SystemDot.Messaging.Sending
{
    public class MessagePayloadPackager
    {
        const string DefaultChannelName = "Default";
        readonly IPipe<MessagePayload> outputPipe;

        public MessagePayloadPackager(IPipe<object> inputPipe, IPipe<MessagePayload> outputPipe)
        {
            Contract.Requires(inputPipe != null);
            Contract.Requires(outputPipe != null);

            inputPipe.ItemPushed += OnInputPipeItemPushed;
            this.outputPipe = outputPipe;
        }

        void OnInputPipeItemPushed(object message)
        {
            var messagePayload = new MessagePayload("http://localhost/" + DefaultChannelName + "/");
            messagePayload.AddHeader(new BodyMessageHeader(message));

            this.outputPipe.Push(messagePayload);
        }
    }
}