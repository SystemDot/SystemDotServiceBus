using SystemDot.Ioc;
using SystemDot.Messaging.Configuration.ExternalSources;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging.Configuration
{
    internal class Components
    {
        public static void Register()
        {
            container.RegisterInstance<IExternalSourcesConfigurer, ExternalSourcesConfigurer>();
        }
    }
}