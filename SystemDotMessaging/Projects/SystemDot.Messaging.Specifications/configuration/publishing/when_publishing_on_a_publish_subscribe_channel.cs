using System;
using System.Linq;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_on_a_publish_subscribe_channel : WithPublisherSubject
    {
        const string ChannelName = "Test";
        const string SubscriberName = "TestSubscriber";
        
        static IBus bus;
        static int message;
        static EndpointAddress subscriberAddress;

        Establish context = () =>
        {    
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPublishing()
                .Initialise();

            message = 1;
            
            subscriberAddress = BuildAddress(SubscriberName);
            Subscribe(subscriberAddress, BuildAddress(ChannelName));
        };

        Because of = () => bus.Publish(message);

        It should_mark_the_message_with_the_persistence_id = () =>
            MessageServer.SentMessages.ExcludeAcknowledgements().First().GetPersistenceId()
                .ShouldEqual(new MessagePersistenceId(
                    MessageServer.SentMessages.ExcludeAcknowledgements().First().Id,
                    subscriberAddress,
                    PersistenceUseType.SubscriberSend));

        It should_set_original_persistence_id_to_the_persistence_id_of_the_message_with_the_persistence_id = () =>
           MessageServer.SentMessages.ExcludeAcknowledgements().First().GetSourcePersistenceId()
               .ShouldEqual(MessageServer.SentMessages.ExcludeAcknowledgements().First().GetPersistenceId());

        It should_send_a_message_with_the_correct_content = () => 
            MessageServer.SentMessages.ExcludeAcknowledgements().First().DeserialiseTo<int>().ShouldEqual(message);

        It should_mark_the_time_the_message_is_sent = () => 
            MessageServer.SentMessages.ExcludeAcknowledgements().First().GetLastTimeSent().ShouldBeGreaterThan(DateTime.MinValue);

        It should_mark_the_amount_of_times_the_message_has_been_sent = () => 
            MessageServer.SentMessages.ExcludeAcknowledgements().First().GetAmountSent().ShouldEqual(1);

        It should_mark_the_message_with_the_sequence = () =>
            MessageServer.SentMessages.ExcludeAcknowledgements().First().GetSequence().ShouldEqual(1);

        It should_mark_the_first_sequence_number_in_the_subscriber_as_one = () =>
            MessageServer.SentMessages.ExcludeAcknowledgements().First().GetFirstSequence().ShouldEqual(1);

        It should_start_the_task_repeater = () => TaskRepeater.Started.ShouldBeTrue();
    }
}