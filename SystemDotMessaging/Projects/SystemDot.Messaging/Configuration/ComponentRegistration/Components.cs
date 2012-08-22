using System;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public class Components 
    {
        public static void Register()
        {
            IIocContainer container = IocContainerLocator.Locate();

            ThreadingComponents.Register(container);
            CoreComponents.Register(container);
            ChannelComponents.Register(container);
            PublishingComponents.Register(container);
            RequestReplyComponents.Register(container);
        }
    }
}