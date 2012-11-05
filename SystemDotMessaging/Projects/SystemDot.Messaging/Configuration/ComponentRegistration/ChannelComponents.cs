using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Acknowledgement.Builders;
using SystemDot.Messaging.Channels.Builders;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class ChannelComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IBus, MessageBus>();
            container.RegisterInstance<MessageHandlerRouter, MessageHandlerRouter>();
            container.RegisterInstance<AcknowledgementChannelBuilder, AcknowledgementChannelBuilder>();
            container.RegisterInstance<MessageAcknowledgementHandler, MessageAcknowledgementHandler>();
            container.RegisterInstance<MessageAcknowledgementHandler, MessageAcknowledgementHandler>();
            container.RegisterInstance<InMemoryChangeStore, InMemoryChangeStore>();
            container.RegisterInstance<PersistenceFactorySelector, PersistenceFactorySelector>();
        }
    }
}