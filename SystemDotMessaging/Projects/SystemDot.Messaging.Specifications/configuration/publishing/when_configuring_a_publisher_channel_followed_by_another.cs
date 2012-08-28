using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject("Publishing configuration")] 
    public class when_configuring_a_publisher_channel_followed_by_another : WithPublisherSubject
    {
        const string Channel2Name = "Test2";

        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel("Test1").ForPublishing()
                .OpenChannel(Channel2Name).ForPublishing()
            .Initialise();

        It should_build_the_second_publisher_channel = () =>
            The<IPublisherChannelBuilder>().As<TestPublisherChannelBuilder>().ExpectedAddress.ShouldEqual(
                GetEndpointAddress(Channel2Name, The<IMachineIdentifier>().GetMachineName()));


    }
}