using SystemDot.Messaging.Publishing.Builders;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_publisher_channel : WithMessageConfigurationSubject
    {
        const string PublisherAddress = "PublisherAddress";

        static PublisherChannelBuilt channelBuiltEvent;

        Because of = () =>
        {
            Messenger.Register<PublisherChannelBuilt>(e => channelBuiltEvent = e);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(PublisherAddress).ForPublishing()
                .Initialise();
        };

        It should_notify_that_the_channel_was_built = () => 
            channelBuiltEvent.Address.ShouldEqual(BuildAddress(PublisherAddress));
    }
}