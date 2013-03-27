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
        static MessageServerConfiguration config;

        Establish context = () =>
        {
            externalSourcesConfigurer = new TestExternalSourcesConfigurer();
            ConfigureAndRegister<IExternalSourcesConfigurer>(externalSourcesConfigurer);
        };

        Because of = () => config = Configuration.Configure.Messaging().UsingInProcessTransport();

        It should_run_any_external_configurations = () => externalSourcesConfigurer.Configuration.ShouldBeTheSameAs(config);   
    }
}