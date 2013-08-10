using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Direct;
using SystemDot.Messaging.Direct.Builders;
using SystemDot.Messaging.Transport.Http;
using MessageReceiverHandler = SystemDot.Messaging.Direct.MessageReceiverHandler;

namespace SystemDot.Messaging.Transport.InProcess.Configuration
{
    class InProcessTransportBuilder : ITransportBuilder
    {
        readonly IInProcessMessageServerFactory messageServerFactory;
        readonly MessageReceiver messageReceiver;
        readonly IIocContainer container;

        public InProcessTransportBuilder(MessageReceiver messageReceiver, IIocContainer container, IInProcessMessageServerFactory messageServerFactory)
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
            container.RegisterInstance(() => messageServerFactory.Create(
                new Http.MessageReceiverHandler(messageReceiver), 
                new MessageReceiverHandler(messageReceiver)));
        }
    }

    
}