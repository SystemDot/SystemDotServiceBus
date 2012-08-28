using SystemDot.Ioc;
using SystemDot.Messaging.Configuration.ComponentRegistration;

namespace SystemDot.Messaging.Configuration
{
    public class Configure : Configurer
    {
        static Configure()
        {
            IocContainerLocator.SetContainer(new IocContainer(new TypeExtender()));
        }

        public static MessagingConfiguration Messaging()
        {
            Components.Register();
            return new MessagingConfiguration();
        }
    }
}