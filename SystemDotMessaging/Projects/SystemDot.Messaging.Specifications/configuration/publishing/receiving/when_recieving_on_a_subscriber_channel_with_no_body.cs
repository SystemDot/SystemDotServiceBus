using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Sequencing;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing.receiving
{
    [Subject(SpecificationGroup.Description)]
    public class when_recieving_on_a_subscriber_channel_with_no_body : WithMessageConfigurationSubject
    {
        const string ChannelName = "TestChannel";
        const string PublisherName = "TestPublisher";
        
        static MessagePayload payload;
        static Exception exception;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingHttpTransport(MessageServer.Local())
                .OpenChannel(ChannelName).ForSubscribingTo(PublisherName)
                .Initialise();

            payload = new MessagePayload();
            payload.SetToAddress(BuildAddress(ChannelName));
            payload.SetFirstSequence(1);
        };

        Because of = () => exception = Catch.Exception(() => MessageReciever.ReceiveMessage(payload));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}