using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class PublishingComponents
    {
        public static void Register(IIocContainer iocContainer)
        {
            iocContainer.RegisterInstance<IPublisherRegistry, PublisherRegistry>();
            iocContainer.RegisterInstance<ISubscriberSendChannelBuilder, SubscriberSendChannelBuilder>();
            iocContainer.RegisterInstance<ISubscriptionHandlerChannelBuilder, SubscriptionHandlerChannelBuilder>();
            iocContainer.RegisterInstance<IPublisherChannelBuilder, PublisherChannelBuilder>();
            iocContainer.RegisterInstance<ISubscriberChannelBuilder, SubscriberRecieveChannelBuilder>();
            iocContainer.RegisterInstance<ISubscriptionRequestChannelBuilder, SubscriptionRequestChannelBuilder>();                
        }
    }
}