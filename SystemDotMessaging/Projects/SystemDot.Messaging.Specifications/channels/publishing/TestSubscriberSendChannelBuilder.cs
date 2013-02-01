using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Publishing.Builders;
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