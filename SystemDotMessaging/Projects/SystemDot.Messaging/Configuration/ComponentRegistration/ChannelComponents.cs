using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Processing.Handling;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class ChannelComponents
    {
        public static void Register()
        {
            IocContainer.RegisterInstance<IBus, MessageBus>();
            IocContainer.RegisterInstance<MessagePayloadCopier, MessagePayloadCopier>();
            IocContainer.RegisterInstance<MessageHandlerRouter, MessageHandlerRouter>();            
        }
    }
}