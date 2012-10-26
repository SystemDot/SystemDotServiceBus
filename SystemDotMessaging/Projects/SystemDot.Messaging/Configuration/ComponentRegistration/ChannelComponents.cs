using SystemDot.Ioc;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Acknowledgement.Builders;
using SystemDot.Messaging.Channels.Builders;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage.InMemory;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class ChannelComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IBus, MessageBus>();
            container.RegisterInstance<MessagePayloadCopier, MessagePayloadCopier>();
            container.RegisterInstance<MessageHandlerRouter, MessageHandlerRouter>();
            container.RegisterInstance<AcknowledgementChannelBuilder, AcknowledgementChannelBuilder>();
            container.RegisterInstance<MessageAcknowledgementHandler, MessageAcknowledgementHandler>();
            container.RegisterInstance<MessageAcknowledgementHandler, MessageAcknowledgementHandler>();
            container.RegisterInstance<InMemoryDatatore, InMemoryDatatore>();
            container.RegisterInstance<PersistenceFactorySelector, PersistenceFactorySelector>();
        }
    }
}