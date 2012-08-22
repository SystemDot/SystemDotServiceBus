using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Processing.Handling;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class ChannelComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IBus, MessageBus>();
            container.RegisterInstance<MessagePayloadCopier, MessagePayloadCopier>();
            container.RegisterInstance<MessageHandlerRouter, MessageHandlerRouter>();            
        }
    }
}