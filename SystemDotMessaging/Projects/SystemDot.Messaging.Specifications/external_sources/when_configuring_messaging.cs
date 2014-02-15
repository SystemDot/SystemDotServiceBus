using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.ExternalSources;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.external_sources
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
            externalSourcesConfigurer.Configuration.Should().BeSameAs(config);
        
        It should_run_any_external_configurations_with_the_correct_server_configuration = () =>
            externalSourcesConfigurer.ServerConfiguration.Should().BeSameAs(serverConfig);   
    }
}