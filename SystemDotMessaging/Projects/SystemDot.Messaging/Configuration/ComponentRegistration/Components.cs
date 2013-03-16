using SystemDot.Ioc;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    class Components 
    {
        public static void Register()
        {
            IIocContainer container = IocContainerLocator.Locate();

            ThreadingComponents.Register(container);
            CoreComponents.Register(container);
            ChannelComponents.Register(container);
            PublishingComponents.Register(container);
            RequestReplyComponents.Register(container);
            PointToPointComponents.Register(container);
        }
    }
}