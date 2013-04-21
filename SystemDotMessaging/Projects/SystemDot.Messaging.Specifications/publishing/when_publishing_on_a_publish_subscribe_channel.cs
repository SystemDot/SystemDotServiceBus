using System;
using System.Linq;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_on_a_publish_subscribe_channel : WithPublisherSubject
    {
        const string ChannelName = "Test";
        const string SubscriberName = "TestSubscriber";
        
        
        static int message;
        static EndpointAddress subscriberAddress;
        
        Establish context = () =>
        {    
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPublishing()
                .Initialise();

            message = 1;
            
            subscriberAddress = BuildAddress(SubscriberName);
            Subscribe(subscriberAddress, BuildAddress(ChannelName));
        };

        Because of = () => Bus.Publish(message);

        It should_mark_the_message_with_the_persistence_id = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().GetPersistenceId()
                .ShouldEqual(new MessagePersistenceId(
                    Server.SentMessages.ExcludeAcknowledgements().First().Id,
                    subscriberAddress,
                    PersistenceUseType.SubscriberSend));

        It should_set_original_persistence_id_to_the_persistence_id_of_the_message_with_the_persistence_id = () =>
           Server.SentMessages.ExcludeAcknowledgements().First().GetSourcePersistenceId()
               .ShouldEqual(Server.SentMessages.ExcludeAcknowledgements().First().GetPersistenceId());

        It should_send_a_message_with_the_correct_content = () => 
            Server.SentMessages.ExcludeAcknowledgements().First().DeserialiseTo<int>().ShouldEqual(message);

        It should_mark_the_time_the_message_is_sent = () => 
            Server.SentMessages.ExcludeAcknowledgements().First().GetLastTimeSent().ShouldBeGreaterThan(DateTime.MinValue);

        It should_mark_the_amount_of_times_the_message_has_been_sent = () => 
            Server.SentMessages.ExcludeAcknowledgements().First().GetAmountSent().ShouldEqual(1);

        It should_mark_the_message_with_the_sequence = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().GetSequence().ShouldEqual(1);

        It should_mark_the_first_sequence_number_in_the_subscriber_as_one = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().GetFirstSequence().ShouldEqual(1);

        It should_start_the_task_repeater = () => TaskRepeater.Started.ShouldBeTrue();
    }

    
}