using SystemDot.Ioc;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Acknowledgement.Builders;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class ChannelComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IBus, MessageBus>();
            container.RegisterInstance<MessagePayloadCopier, MessagePayloadCopier>();
            container.RegisterInstance<MessageHandlerRouter, MessageHandlerRouter>();
            container.RegisterInstance<IAcknowledgementChannelBuilder, AcknowledgementChannelBuilder>();
            container.RegisterInstance<MessageCacheBuilder, MessageCacheBuilder>();            
        }
    }
}