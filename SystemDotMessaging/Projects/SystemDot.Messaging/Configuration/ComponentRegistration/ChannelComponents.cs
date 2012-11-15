using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Acknowledgement.Builders;
using SystemDot.Messaging.Channels.Builders;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Local.Builders;
using SystemDot.Messaging.Channels.UnitOfWork;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class ChannelComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IBus, MessageBus>();
            container.RegisterInstance<MessageHandlerRouter, MessageHandlerRouter>();
            container.RegisterInstance<LocalChannelBuilder, LocalChannelBuilder>();
            container.RegisterInstance<AcknowledgementSendChannelBuilder, AcknowledgementSendChannelBuilder>();
            container.RegisterInstance<AcknowledgementRecieveChannelBuilder, AcknowledgementRecieveChannelBuilder>();
            container.RegisterInstance<MessageAcknowledgementHandler, MessageAcknowledgementHandler>();
            container.RegisterInstance<AcknowledgementSender, AcknowledgementSender>();
            container.RegisterInstance<IPersistenceFactory, PersistenceFactory>();
            container.RegisterInstance<InMemoryChangeStore, InMemoryChangeStore>();
            container.RegisterInstance<PersistenceFactorySelector, PersistenceFactorySelector>();
            container.RegisterInstance<IUnitOfWork, NullUnitOfWork>();
        }
    }
}