using SystemDot.Configuration;
using SystemDot.Files;
using SystemDot.Http;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.ExternalSources;
using SystemDot.Messaging.Hooks.External;
using SystemDot.Messaging.Hooks.Upgrading;
using SystemDot.Messaging.Transport;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Upcasting;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    static class CoreComponents
    {
        public static void Register(IIocContainer container)
        {
            container.RegisterInstance<IIocContainer>(() => container);
            container.RegisterInstance<IExternalSourcesConfigurer, ExternalSourcesConfigurer>();
            container.RegisterInstance<IFileSystem, FileSystem>();
            container.RegisterInstance<IWebRequestor, WebRequestor>();
            container.RegisterInstance<EndpointAddressBuilder, EndpointAddressBuilder>();
            container.RegisterInstance<MessageServerBuilder, MessageServerBuilder>();
            container.RegisterInstance<ISystemTime>(() => SystemTime.Current);
            container.RegisterInstance<IConfigurationReader, ConfigurationReader>();
            container.RegisterInstance<ServerAddressLoader, ServerAddressLoader>();
            container.RegisterInstance<ServerAddressRegistry, ServerAddressRegistry>();
            container.RegisterInstance<MessageSender, MessageSender>();
            container.RegisterInstance<MessageReceiver, MessageReceiver>();
            container.RegisterInstance<NullChangeStore, NullChangeStore>();
            container.RegisterInstance<ChangeUpcasterRunner, ChangeUpcasterRunner>();
            container.RegisterInstance<ICheckpointStrategy, CheckpointAfterOneThousandChangesCheckpointStrategy>();
            container.RegisterInstance<UpgradeMessageHook, UpgradeMessageHook>();
            container.RegisterInstance<ApplicationTypeActivator, ApplicationTypeActivator>();
            container.RegisterInstance<ExternalInspectorHook, ExternalInspectorHook>();
            container.RegisterInstance<IExternalInspectorLoader, ExternalInspectorLoader>();
        }
    }
}