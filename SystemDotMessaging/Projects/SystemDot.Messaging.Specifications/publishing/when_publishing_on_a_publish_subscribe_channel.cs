using System;
using System.Linq;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using FluentAssertions;
using Machine.Specifications;using FluentAssertions;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_on_a_publish_subscribe_channel : WithPublisherSubject
    {
        const string ChannelName = "Test";
        const string SubscriberName = "TestSubscriber";

        static Int64 message;
        static EndpointAddress subscriberAddress;
        
        Establish context = () =>
        {    
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPublishing()
                .Initialise();

            message = 1;
            
            subscriberAddress = BuildAddress(SubscriberName);
            Subscribe(subscriberAddress, BuildAddress(ChannelName));
        };

        Because of = () => Bus.Publish(message);

        It should_mark_the_message_with_the_persistence_id = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetPersistenceId()
                .ShouldBeEquivalentTo(new MessagePersistenceId(
                    GetServer().SentMessages.ExcludeAcknowledgements().First().Id,
                    subscriberAddress,
                    PersistenceUseType.SubscriberSend));

        It should_set_original_persistence_id_to_the_persistence_id_of_the_message_with_the_persistence_id = () =>
           GetServer().SentMessages.ExcludeAcknowledgements().First().GetSourcePersistenceId()
               .ShouldBeEquivalentTo(GetServer().SentMessages.ExcludeAcknowledgements().First().GetPersistenceId());

        It should_send_a_message_with_the_correct_content = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().DeserialiseTo<Int64>().ShouldBeEquivalentTo(message);

        It should_mark_the_time_the_message_is_sent = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetLastTimeSent().Should().BeOnOrAfter(DateTime.MinValue);

        It should_mark_the_amount_of_times_the_message_has_been_sent = () => 
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetAmountSent().ShouldBeEquivalentTo(1);

        It should_mark_the_message_with_the_sequence = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetSequence().ShouldBeEquivalentTo(1);

        It should_mark_the_first_sequence_number_in_the_subscriber_as_one = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetFirstSequence().ShouldBeEquivalentTo(1);

        It should_start_the_task_repeater = () => TaskRepeater.Started.Should().BeTrue();
    }

    
}