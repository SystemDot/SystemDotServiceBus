using System.Diagnostics.Contracts;
using SystemDot.Pipes;

namespace SystemDot.Messaging
{
    public class MessageBus
    {
        public static void Initialise(IPipe<object> outputPipe)
        {
            Contract.Requires(outputPipe != null);

            instance = new MessageBus(outputPipe);
        }

        public static void Send(object message)
        {
            Contract.Requires(message != null);

            instance.SendMessage(message);
        }

        readonly IPipe<object> outputPipe;
        static MessageBus instance;

        MessageBus(IPipe<object> outputPipe)
        {
            this.outputPipe = outputPipe;
        }

        void SendMessage(object message)
        {
            this.outputPipe.Push(message);
        }
    }
}