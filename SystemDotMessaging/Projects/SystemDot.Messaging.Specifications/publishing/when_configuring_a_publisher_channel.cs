using SystemDot.Messaging.Publishing.Builders;
using SystemDot.Messaging.Simple;
using Machine.Specifications;using FluentAssertions;

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

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(PublisherAddress).ForPublishing()
                .Initialise();
        };

        It should_notify_that_the_channel_was_built = () => 
            channelBuiltEvent.Address.ShouldBeEquivalentTo(BuildAddress(PublisherAddress));
    }
}