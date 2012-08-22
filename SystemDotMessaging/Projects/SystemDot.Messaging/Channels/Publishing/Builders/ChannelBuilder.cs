using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class ChannelBuilder : IChannelBuilder 
    {
        readonly IMessageSender messageSender;

        public ChannelBuilder(IMessageSender messageSender)
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