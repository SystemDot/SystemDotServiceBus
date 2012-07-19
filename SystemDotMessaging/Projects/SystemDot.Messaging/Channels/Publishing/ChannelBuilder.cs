using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class ChannelBuilder : IChannelBuilder 
    {
        public IMessageInputter<MessagePayload> Build(SubscriptionSchema toSchema) 
        {
            MessageAddresser addresser = IocContainer
                .Resolve<MessageAddresser, EndpointAddress>(toSchema.SubscriberAddress);

            MessagePipelineBuilder.Build()
                .With(addresser)
                .ToEndPoint(IocContainer.Resolve<IMessageSender>());

            return addresser;
        }
    }
}