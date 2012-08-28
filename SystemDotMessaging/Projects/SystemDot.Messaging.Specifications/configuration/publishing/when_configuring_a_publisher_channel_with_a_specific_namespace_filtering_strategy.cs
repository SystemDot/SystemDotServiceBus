using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Messages.Processing.Filtering;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")]
    public class when_configuring_a_publisher_channel_with_a_specific_namespace_filtering_strategy : WithPublisherSubject
    {
        const string Channel2Name = "Test2";

        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel("Test1").ForPublishing().OnlyForMessages(FilteredBy.Namespace("Namespace"))
            .Initialise();

        It should_build_the_publisher_channel_with_the_default_pass_through_message_filterer = () =>
            The<IPublisherChannelBuilder>().As<TestPublisherChannelBuilder>().ExpectedMessageFilterStrategy
                .ShouldBeOfType<NamespaceMessageFilterStrategy>();


    }
}