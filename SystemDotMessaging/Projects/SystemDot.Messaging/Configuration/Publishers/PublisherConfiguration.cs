using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration.Publishers
{
    public class PublisherConfiguration : Configurer
    {
        readonly EndpointAddress address;

        public PublisherConfiguration(EndpointAddress address)
        {
            this.address = address;
        }

        public IBus Initialise()
        {
            BuildSubscriptionRequestHandler();
            BuildPublisher();

            IocContainer.Resolve<ITaskLooper>().Start();
            return IocContainer.Resolve<IBus>();
        }

        void BuildSubscriptionRequestHandler()
        {
            MessagePipelineBuilder.Build()
                .With(Resolve<IMessageReciever>())
                .Pump()
                .ToEndPoint(Resolve<SubscriptionRequestHandler>());

            Resolve<IMessageReciever>().RegisterListeningAddress(address);
        }

        void BuildPublisher()
        {
            var publisher = Resolve<IDistributor>();
            Resolve<IPublisherRegistry>().RegisterPublisher(address, publisher);

            MessagePipelineBuilder.Build()
                .With(Resolve<IBus>())
                .Pump()
                .ToProcessor(Resolve<MessagePayloadPackager>())
                .ToEndPoint(publisher);
        }
    }
}