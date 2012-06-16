using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using SystemDot.Pipes;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Sending
{
    public class MessagePayloadPackager
    {
        const string DefaultChannelName = "Default";
        readonly IPipe<MessagePayload> outputPipe;
        private readonly ISerialiser serialiser;

        public MessagePayloadPackager(IPipe<object> inputPipe, IPipe<MessagePayload> outputPipe, ISerialiser serialiser)
        {
            Contract.Requires(inputPipe != null);
            Contract.Requires(outputPipe != null);
            Contract.Requires(serialiser != null);

            inputPipe.ItemPushed += OnInputPipeItemPushed;
            this.outputPipe = outputPipe;
            this.serialiser = serialiser;
        }

        void OnInputPipeItemPushed(object message)
        {
            var messagePayload = new MessagePayload(Address.Default);
            messagePayload.SetBody(this.serialiser.Serialise(message));

            this.outputPipe.Push(messagePayload);
        }
    }
}