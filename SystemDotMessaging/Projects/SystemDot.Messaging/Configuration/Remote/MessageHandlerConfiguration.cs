using System.Diagnostics.Contracts;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Http;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.Recieving;
using SystemDot.Pipes;
using SystemDot.Serialisation;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.Remote
{
    public class MessageHandlerConfiguration
    {
        const string DefaultChannelName = "Default";

        readonly ThreadedWorkCoordinator workCoordinator;
        readonly ThreadPool threadPool;
        readonly IMessageHandler toRegister;
        
        public MessageHandlerConfiguration(
            ThreadedWorkCoordinator workCoordinator, 
            ThreadPool threadPool, 
            IMessageHandler toRegister)
        {
            Contract.Requires(workCoordinator != null);
            Contract.Requires(threadPool != null);
            Contract.Requires(toRegister != null);

            this.workCoordinator = workCoordinator;
            this.threadPool = threadPool;
            this.toRegister = toRegister;
        }

        public void Initialise()
        {
            IPipe<MessagePayload> payloadPipe = BuildPayloadPipe();
            IPipe<object> messagePipe = BuildMessagePipe();

            LongPollReciever longPollReciever = BuildLongPollReciever(Address.Default, payloadPipe);
            BuildPayloadPackager(payloadPipe, messagePipe);
            BuildHandlerRouter(messagePipe, this.toRegister);
            
            this.workCoordinator.RegisterWorker(longPollReciever);
            this.workCoordinator.Start();
        }

        private IPipe<object> BuildMessagePipe()
        {
            return new Pipe<object>();
        }

        private IPipe<MessagePayload> BuildPayloadPipe()
        {
            return new Pump<MessagePayload>(this.threadPool);
        }

        LongPollReciever BuildLongPollReciever(Address address, IPipe<MessagePayload> pipe)
        {
            return new LongPollReciever(address, pipe, new WebRequestor(), new BinaryFormatter());
        }

        private static void BuildPayloadPackager(IPipe<MessagePayload> inputPipe, IPipe<object> outputPipe)
        {
            new MessagePayloadUnpackager(inputPipe, outputPipe, new BinarySerialiser(new BinaryFormatter()));
        }

        void BuildHandlerRouter(IPipe<object> pipe, IMessageHandler register)
        {
            new MessageHandlerRouter(pipe).RegisterHandler(register);
        }
    }
}