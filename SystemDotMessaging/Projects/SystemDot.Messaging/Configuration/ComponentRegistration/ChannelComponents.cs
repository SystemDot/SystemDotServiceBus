using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Processing.Handling;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class ChannelComponents
    {
        public static void Register()
        {
            IocContainer.Register<IBus>(new MessageBus());
            
            IocContainer.Register(new MessagePayloadCopier(IocContainer.Resolve<ISerialiser>()));
            IocContainer.Register(new MessageHandlerRouter());            
        }
    }
}