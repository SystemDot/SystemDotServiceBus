using SystemDot.Messaging.Publishing.Builders;
using SystemDot.Messaging.Simple;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_publisher_channel : WithMessageConfigurationSubject
    {
        const string PublisherAddress = "PublisherAddress";

        static PublisherChannelBuilt channelBuiltEvent;

        Because of = () =>
        {
            Messenger.RegisterHandler<PublisherChannelBuilt>(e => channelBuiltEvent = e);

            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(PublisherAddress).ForPublishing()
                .Initialise();
        };

        It should_notify_that_the_channel_was_built = () => 
            channelBuiltEvent.Address.ShouldEqual(BuildAddress(PublisherAddress));
    }
}