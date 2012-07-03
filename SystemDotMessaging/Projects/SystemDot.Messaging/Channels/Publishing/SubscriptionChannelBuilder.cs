using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http.LongPolling;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscriptionChannelBuilder : ISubscriptionChannelBuilder 
    {
        public IMessageInputter<MessagePayload> Build(SubscriptionSchema toSchema) 
        {
            MessageAddresser addresser = MessagingEnvironment
                .GetComponent<MessageAddresser, EndpointAddress>(toSchema.SubscriberAddress);

            MessagePipelineBuilder.Build()
                .With(addresser)
                .ToEndPoint(MessagingEnvironment.GetComponent<IMessageSender>());

            return addresser;
        }
    }
}