using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Processing.Handling;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class PublishingComponents
    {
        public static void Register()
        {
            IocContainer.Register<IPublisherRegistry>(new PublisherRegistry());

            IocContainer.Register<IChannelBuilder>(new ChannelBuilder(IocContainer.Resolve<IMessageSender>()));

            IocContainer.Register(
                new SubscriptionRequestHandler(
                    IocContainer.Resolve<IPublisherRegistry>(),
                    IocContainer.Resolve<IChannelBuilder>()));

            IocContainer.Register<ISubscriptionHandlerChannelBuilder>(
                new SubscriptionHandlerChannelBuilder(
                    IocContainer.Resolve<SubscriptionRequestHandler>(),
                    IocContainer.Resolve<IMessageReciever>()));
            
            IocContainer.Register<IPublisherChannelBuilder>(
                new PublisherChannelBuilder(
                    IocContainer.Resolve<IPublisherRegistry>(), 
                    IocContainer.Resolve<MessagePayloadCopier>(),
                    IocContainer.Resolve<ISerialiser>()));

            IocContainer.Register<ISubscriberChannelBuilder>(
                new SubscriberChannelBuilder(
                    IocContainer.Resolve<ISerialiser>(), 
                    IocContainer.Resolve<MessageHandlerRouter>(),
                    IocContainer.Resolve<IMessageReciever>()));
            
            IocContainer.Register<ISubscriptionRequestChannelBuilder>(
                new SubscriptionRequestChannelBuilder(
                    IocContainer.Resolve<IMessageSender>(), 
                    IocContainer.Resolve<ITaskScheduler>()));
                
        }
    }
}