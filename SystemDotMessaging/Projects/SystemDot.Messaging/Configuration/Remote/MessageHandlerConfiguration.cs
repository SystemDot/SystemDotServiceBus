using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using SystemDot.Http;
using SystemDot.Messaging.Channels.Remote;
using SystemDot.Messaging.Configuration.Channels;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Serialisation;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.Remote
{
    public class MessageHandlerConfiguration
    {
        readonly IMessageHandler toRegister;
        
        public MessageHandlerConfiguration(IMessageHandler toRegister)
        {
            Contract.Requires(toRegister != null);
            this.toRegister = toRegister;
        }

        public void Initialise()
        {
            ChannelBuilder
                .StartsWith(BuildLongPollReciever(Address.Default))
                .Pump()
                .ToProcessor(BuildPayloadPackager())
                .ThenToEndPoint(BuildHandlerRouter(this.toRegister));

            MessagingEnvironment.GetComponent<ThreadedWorkCoordinator>().Start();
        }

        LongPollReciever BuildLongPollReciever(Address address)
        {
            var longPollReciever = new LongPollReciever(
                address, 
                MessagingEnvironment.GetComponent<IWebRequestor>(),
                MessagingEnvironment.GetComponent<IFormatter>());

            MessagingEnvironment.GetComponent<ThreadedWorkCoordinator>().RegisterWorker(longPollReciever);

            return longPollReciever;
        }

        private static MessagePayloadUnpackager BuildPayloadPackager()
        {
            return new MessagePayloadUnpackager(MessagingEnvironment.GetComponent<ISerialiser>());
        }

        MessageHandlerRouter BuildHandlerRouter(IMessageHandler register)
        {
            var router = new MessageHandlerRouter();
            router.RegisterHandler(register);
            return router;
        }
    }
}