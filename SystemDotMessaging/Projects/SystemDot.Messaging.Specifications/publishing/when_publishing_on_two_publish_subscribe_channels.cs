using System;
using System.Linq;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_on_two_publish_subscribe_channels 
        : WithPublisherSubject
    {
        const string Channel1Name = "Test1";
        const string Subscriber1Name = "TestSubscriber1";
        const string Channel2Name = "Test2";
        const string Subscriber2Name = "TestSubscriber2";
        
        static Int64 message;
        
        Establish context = () =>
        {
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(Channel1Name).ForPublishing()
                .OpenChannel(Channel2Name).ForPublishing()
                .Initialise();

            message = 1;

            Subscribe(BuildAddress(Subscriber1Name), BuildAddress(Channel1Name));
            Subscribe(BuildAddress(Subscriber2Name), BuildAddress(Channel2Name));
        };

        Because of = () => Bus.Publish(message);

        It should_publish_a_message_with_the_correct_content_through_both_channels = () =>
            GetServer().SentMessages.ExcludeAcknowledgements()
                .Count(m => m.DeserialiseTo<Int64>() == message).ShouldBeEquivalentTo(2);
    }
}