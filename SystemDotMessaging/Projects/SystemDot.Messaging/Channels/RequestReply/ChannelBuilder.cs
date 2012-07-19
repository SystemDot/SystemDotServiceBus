using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Consuming;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class ChannelBuilder : IChannelBuilder 
    {
        public void Build(SubscriptionSchema toSchema) 
        {
            BuildSendChannel(toSchema);
            BuildRecieveChannel();           
        }

        void BuildSendChannel(SubscriptionSchema toSchema)
        {
            MessagePipelineBuilder.Build()
               .With(IocContainer.Resolve<IBus>())
               .Pump()
               .ToProcessor(IocContainer.Resolve<MessagePayloadPackager>())
               .ToProcessor(IocContainer.Resolve<MessageAddresser, EndpointAddress>(toSchema.SubscriberAddress))
               .ToEndPoint(IocContainer.Resolve<IMessageSender>());
        }

        void BuildRecieveChannel()
        {
            MessagePipelineBuilder.Build()
               .With(IocContainer.Resolve<IMessageReciever>())
               .Pump()
               .ToProcessor(IocContainer.Resolve<MessagePayloadUnpackager>())
               .ToEndPoint(IocContainer.Resolve<MessageHandlerRouter>());
        }
    }
}