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
        readonly string channel;

        public PublisherConfiguration(string channel)
        {
            Contract.Requires(!string.IsNullOrEmpty(channel));
            this.channel = channel;
        }

        public override IBus Initialise()
        {
            Components.Register();
            var address = new EndpointAddress(this.channel, Resolve<IMachineIdentifier>().GetMachineName());
            BuildSubscriptionRequestHandler(address);
            BuildPublisher(address);

            IocContainer.Resolve<TaskLooper>().Start();
            return IocContainer.Resolve<IBus>();
        }

        static void BuildSubscriptionRequestHandler(EndpointAddress address)
        {
            MessagePipelineBuilder.Build()
                .With(Resolve<IMessageReciever>())
                .Pump()
                .ToEndPoint(Resolve<SubscriptionRequestHandler>());

            Resolve<IMessageReciever>().RegisterListeningAddress(address);
        }

        static void BuildPublisher(EndpointAddress address)
        {
            BuildPublisherChannel(address);
        }

        static void BuildPublisherChannel(EndpointAddress address)
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