using SystemDot.Ioc;
using SystemDot.Messaging.Configuration.ComponentRegistration;

namespace SystemDot.Messaging.Configuration
{
    public class Configure : Configurer
    {
        public static MessagingConfiguration Messaging()
        {
            return Messaging(new IocContainer(new TypeExtender()));
        }

        public static MessagingConfiguration Messaging(IIocContainer container)
        {
            IocContainerLocator.SetContainer(container);
            Components.Register();
            return new MessagingConfiguration();
        }
    }
}