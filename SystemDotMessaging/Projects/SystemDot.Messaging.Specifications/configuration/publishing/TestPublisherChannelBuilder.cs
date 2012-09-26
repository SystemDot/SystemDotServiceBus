using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Publishing.Builders;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    public class TestPublisherChannelBuilder : IPublisherChannelBuilder
    {
        public EndpointAddress ExpectedAddress { get; private set; }

        public IMessageFilterStrategy ExpectedMessageFilterStrategy { get; private set; }

        public void Build(PublisherChannelSchema schema)
        {
            this.ExpectedAddress = schema.Address;
            this.ExpectedMessageFilterStrategy = schema.MessageFilterStrategy;
        }
    }
}