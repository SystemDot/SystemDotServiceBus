using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class ChannelBuilder : IChannelBuilder 
    {
        readonly IMessageInputter<MessagePayload> messageSender;

        public ChannelBuilder(IMessageInputter<MessagePayload> messageSender)
        {
            this.messageSender = messageSender;
        }

        public IMessageInputter<MessagePayload> Build(EndpointAddress fromAddress, EndpointAddress subscriberAddress) 
        {
            var addresser = new MessageAddresser(fromAddress, subscriberAddress);

            MessagePipelineBuilder.Build()
                .With(addresser)
                .ToEndPoint(this.messageSender);

            return addresser;
        }
    }
}