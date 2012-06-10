using System.Diagnostics.Contracts;
using SystemDot.Messaging.Pipes;

namespace SystemDot.Messaging
{
    public class MessageBus
    {
        public static void Initialise(IPipe outputPipe)
        {
            Contract.Requires(outputPipe != null);
            instance = new MessageBus(outputPipe);
        }

        public static void Send(object message)
        {
            Contract.Requires(message != null);
            instance.SendMessage(message);
        }

        readonly IPipe outputPipe;
        static MessageBus instance;

        MessageBus(IPipe outputPipe)
        {
            this.outputPipe = outputPipe;
        }

        void SendMessage(object message)
        {
            this.outputPipe.Publish(message);
        }
    }
}