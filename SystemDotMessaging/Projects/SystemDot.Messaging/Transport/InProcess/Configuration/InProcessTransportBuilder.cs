using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Transport.Http;

namespace SystemDot.Messaging.Transport.InProcess.Configuration
{
    class InProcessTransportBuilder : ITransportBuilder
    {
        readonly IInProcessMessageServerFactory messageServerFactory;
        readonly IMessageReceiver messageReceiver;
        readonly IIocContainer container;

        public InProcessTransportBuilder(IMessageReceiver messageReceiver, IIocContainer container, IInProcessMessageServerFactory messageServerFactory)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(container != null);
            Contract.Requires(messageServerFactory != null);

            this.messageReceiver = messageReceiver;
            this.container = container;
            this.messageServerFactory = messageServerFactory;
        }

        public void Build(MessageServer toListenFor) 
        {
            container.RegisterInstance(() => messageServerFactory.Create(new MessageReceiverHandler(messageReceiver)));
        }
    }
}