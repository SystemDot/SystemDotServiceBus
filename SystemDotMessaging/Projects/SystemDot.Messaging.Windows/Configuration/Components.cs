using SystemDot.Environment.Configuration;
using SystemDot.Files.Configuration;
using SystemDot.Http.Builders;
using SystemDot.Ioc;
using SystemDot.Messaging.Configuration.ExternalSources;
using SystemDot.Messaging.Ioc;
using SystemDot.Parallelism;
using SystemDot.ThreadMarshalling;

namespace SystemDot.Messaging.Configuration
{
    internal class Components
    {
        public static void Register()
        {
            IIocContainer container = IocContainerLocator.Locate();

            container.RegisterEnvironment();
            container.RegisterFileSystem();
            container.RegisterInstance<IHttpServerBuilder, HttpServerBuilder>();
            container.RegisterInstance<IExternalSourcesConfigurer, ExternalSourcesConfigurer>();
            container.RegisterInstance<ITaskScheduler, TaskScheduler>();
            container.RegisterInstance<IMainThreadMarshaller, MainThreadMarshaller>();
        }
    }
}