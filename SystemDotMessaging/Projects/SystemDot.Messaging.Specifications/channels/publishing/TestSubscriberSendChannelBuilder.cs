using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    public class TestSubscriberSendChannelBuilder : ISubscriberSendChannelBuilder
    {
        readonly IMessageSender sender;

        public TestSubscriberSendChannelBuilder(IMessageSender sender)
        {
            this.sender = sender;
        }

        public IMessageInputter<MessagePayload> BuildChannel(SubscriberSendChannelSchema schema)
        {
            var addresser = new MessageAddresser(schema.FromAddress, schema.SubscriberAddress);
      
            MessagePipelineBuilder.Build()
                .With(addresser)
                .ToEndPoint(this.sender);

            return addresser;
        }
    }
}