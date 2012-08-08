using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class PublisherChannelBuilder : IPublisherChannelBuilder
    {
        public IDistributor Build()
        {
            var publisherEndpoint = IocContainer.Resolve<IDistributor>();

            MessagePipelineBuilder.Build()
                .WithBusPublishTo(IocContainer.Resolve<MessageFilter>())
                .Pump()
                .ToConverter(IocContainer.Resolve<MessagePayloadPackager>())
                .ToEndPoint(publisherEndpoint);

            return publisherEndpoint;
        }
    }
}