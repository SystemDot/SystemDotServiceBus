using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.ExternalSources;
using Machine.Specifications;
using SystemDot.Messaging.Transport.InProcess.Configuration;

namespace SystemDot.Messaging.Specifications.channels.configuration
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_messaging : WithConfigurationSubject
    {
        static TestExternalSourcesConfigurer externalSourcesConfigurer;
        static MessagingConfiguration config;
        static MessageServerConfiguration serverConfig;

        Establish context = () =>
        {
            externalSourcesConfigurer = new TestExternalSourcesConfigurer();
            ConfigureAndRegister<IExternalSourcesConfigurer>(externalSourcesConfigurer);
        };

        Because of = () =>
        {
            config = Configuration.Configure.Messaging();
            serverConfig = config.UsingInProcessTransport();
        };

        It should_run_any_external_configurations_with_the_correct_configuration = () =>
            externalSourcesConfigurer.Configuration.ShouldBeTheSameAs(config);
        
        It should_run_any_external_configurations_with_the_correct_server_configuration = () =>
            externalSourcesConfigurer.ServerConfiguration.ShouldBeTheSameAs(serverConfig);   
    }
}