using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Configuration.HttpMessaging;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Processing.Filtering;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")]
    public class when_configuring_a_publisher_channel_with_a_specific_namespace_filtering_strategy : WithConfiguationSubject
    {
        const string Channel2Name = "Test2";
        
        Establish context = () =>
        {
            Components.Registration = () =>
            {
                ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
                ConfigureAndRegister(new EndpointAddressBuilder(IocContainer.Resolve<IMachineIdentifier>()));
                ConfigureAndRegister<ISubscriptionHandlerChannelBuilder>();
                ConfigureAndRegister<IPublisherRegistry>();
                ConfigureAndRegister<IPublisherChannelBuilder>(new TestPublisherChannelBuilder());
                ConfigureAndRegister<IMessageReciever>();
                ConfigureAndRegister<ITaskLooper>();
                ConfigureAndRegister<IBus>();
            };
        };

        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel("Test1").ForPublishing().OnlyForMessages(FilteredBy.Namespace("Namespace"))
            .Initialise();

        It should_build_the_publisher_channel_with_the_default_pass_through_message_filterer = () =>
            The<IPublisherChannelBuilder>().As<TestPublisherChannelBuilder>().ExpectedMessageFilterStrategy
                .ShouldBeOfType<NamespaceMessageFilterStrategy>();


    }
}