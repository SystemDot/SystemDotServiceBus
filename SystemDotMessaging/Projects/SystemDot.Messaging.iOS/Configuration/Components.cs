using SystemDot.Environment.Configuration;
using SystemDot.Files.Configuration;
using SystemDot.Http.Configuration;
using SystemDot.Ioc;
using SystemDot.Messaging.Configuration.ExternalSources;
using SystemDot.Messaging.Ioc;
using SystemDot.Parallelism.Configuration;
using SystemDot.ThreadMarshalling.Configuration;

namespace SystemDot.Messaging.Configuration
{
    internal class Components
    {
        public static void Register()
        {
            IIocContainer container = IocContainerLocator.Locate();

            container.RegisterEnvironment();
            container.RegisterFileSystem();
            container.RegisterHttp();
            container.RegisterParallelism();
            container.RegisterThreadMarshalling();
            container.RegisterInstance<IExternalSourcesConfigurer, ExternalSourcesConfigurer>();
        }
    }
}