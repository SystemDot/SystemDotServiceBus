using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Http;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.Sending;
using SystemDot.Pipes;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.Local
{
    public class UsingDefaultsConfiguration
    {
        readonly ThreadPool threadPool;
        
        public UsingDefaultsConfiguration(ThreadPool threadPool)
        {
            Contract.Requires(threadPool != null);
            this.threadPool = threadPool;
        }

        public void Initialise()
        {
            IPipe<object> messagePipe = BuildMessagePipe();
            IPipe<MessagePayload> payloadPipe = BuildPayloadPipe();
            BuildMessageBus(messagePipe);
            BuildPayloadPackager(messagePipe, payloadPipe);
            BuildMessageSender(payloadPipe);
        }

        private IPipe<object> BuildMessagePipe()
        {
            return new Pipe<object>();
        }

        private IPipe<MessagePayload> BuildPayloadPipe()
        {
            return new Pump<MessagePayload>(this.threadPool);
        }

        private static void BuildMessageBus(IPipe<object> pipe)
        {
            MessageBus.Initialise(pipe);
        }

        private static void BuildPayloadPackager(IPipe<object> inputPipe, IPipe<MessagePayload> outputPipe)
        {
            new MessagePayloadPackager(inputPipe, outputPipe);
        }

        private void BuildMessageSender(IPipe<MessagePayload> pipe)
        {    
            new MessageSender(pipe, new BinaryFormatter(), new WebRequestor());
        }
    }
}