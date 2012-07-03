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
    public class PublisherConfiguration : InitialisingConfiguration
    {
        readonly EndpointAddress address;

        public PublisherConfiguration(EndpointAddress address)
        {
            Contract.Requires(address != EndpointAddress.Empty);
            this.address = address;
        }

        public override void Initialise()
        {
            Components.Register();
            BuildSubscriptionRequestHandler(address);
            BuildPublisher(address);

            GetComponent<AsynchronousWorkCoordinator>().Start();
        }

        static void BuildSubscriptionRequestHandler(EndpointAddress address)
        {
            MessagePipelineBuilder.Build()
                .With(GetComponent<IMessageReciever>())
                .Pump()
                .ToEndPoint(GetComponent<SubscriptionRequestHandler>());

            GetComponent<IMessageReciever>().RegisterListeningAddress(address);
        }

        static void BuildPublisher(EndpointAddress address)
        {
            BuildPublisherChannel(address);
        }

        static void BuildPublisherChannel(EndpointAddress address)
        {
            var publisher = GetComponent<IDistributor>();
            GetComponent<IPublisherRegistry>().RegisterPublisher(address, publisher);

            MessagePipelineBuilder.Build()
                .With(GetComponent<MessageBus>())
                .Pump()
                .ToProcessor(GetComponent<MessagePayloadPackager>())
                .ToEndPoint(publisher);
        }
    }
}