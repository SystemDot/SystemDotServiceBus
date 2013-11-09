using SystemDot.Ioc;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Acknowledgement.Builders;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Correlation;
using SystemDot.Messaging.Direct.Builders;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Storage;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    static class ChannelComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IBus, MessageBus>();
            container.RegisterInstance<MessageHandlerRouter, MessageHandlerRouter>();
            container.RegisterInstance<LocalDirectChannelBuilder, LocalDirectChannelBuilder>();
            container.RegisterInstance<AcknowledgementSendChannelBuilder, AcknowledgementSendChannelBuilder>();
            container.RegisterInstance<AcknowledgementRecieveChannelBuilder, AcknowledgementRecieveChannelBuilder>();
            container.RegisterInstance<MessageAcknowledgementHandler, MessageAcknowledgementHandler>();
            container.RegisterInstance<AcknowledgementSender, AcknowledgementSender>();
            container.RegisterInstance<MessageCacheFactory, MessageCacheFactory>();
            container.RegisterInstance<ChangeStoreSelector, ChangeStoreSelector>();
            container.RegisterInstance<ReplyCorrelationLookup, ReplyCorrelationLookup>();
            container.RegisterInstance<CorrelationLookup, CorrelationLookup>();
        }
    }
}