using System.Diagnostics.Contracts;
using SystemDot.Messaging.Pipes;
using SystemDot.Messaging.Recieving;

namespace SystemDot.Messaging.Configuration.RequestReply.Client
{
    public class ConsumerConfiguration
    {
        readonly IPipe inputPipe;
        readonly ConsumerMessageBroadcaster broadcaster;

        public ConsumerConfiguration(IPipe inputPipe, ConsumerMessageBroadcaster broadcaster)
        {
            Contract.Requires(inputPipe != null);
            Contract.Requires(broadcaster != null);

            this.inputPipe = inputPipe;
            this.broadcaster = broadcaster;
        }

        public void ConsumingMessagesWith<T>(IConsume<T> toRegister)
        {
            Contract.Requires(toRegister != null);

            broadcaster.RegisterConsumer(toRegister);
        }
    }
}