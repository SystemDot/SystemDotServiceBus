using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class ChannelBuilder : IChannelBuilder 
    {
        public IMessageInputter<MessagePayload> Build(EndpointAddress fromAddress, EndpointAddress subscriberAddress) 
        {
            MessageAddresser addresser = IocContainer.Resolve<MessageAddresser, EndpointAddress, EndpointAddress>(
                fromAddress,
                subscriberAddress);

            MessagePipelineBuilder.Build()
                .With(addresser)
                .ToEndPoint(IocContainer.Resolve<IMessageSender>());

            return addresser;
        }
    }
}