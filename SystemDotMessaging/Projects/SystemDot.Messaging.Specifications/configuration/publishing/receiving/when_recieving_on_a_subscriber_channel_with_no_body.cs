using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Messaging.Transport.InProcess.Configuration;
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
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForSubscribingTo(PublisherName)
                .Initialise();

            payload = new MessagePayload();
            payload.SetToAddress(BuildAddress(ChannelName));
            payload.SetFirstSequence(1);
        };

        Because of = () => exception = Catch.Exception(() => MessageServer.ReceiveMessage(payload));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}