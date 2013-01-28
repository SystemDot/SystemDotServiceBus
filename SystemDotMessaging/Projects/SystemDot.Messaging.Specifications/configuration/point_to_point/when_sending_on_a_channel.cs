using System;
using System.Linq;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Specifications.configuration.publishing;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.configuration.point_to_point
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_on_a_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverName = "TestReceiver";
        
        static IBus bus;
        static int message;
        
        Establish context = () =>
        {    
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPointToPointSendingTo(ReceiverName)
                .Initialise();

            message = 1;
        };

        Because of = () => bus.Send(message);

        It should_mark_the_message_with_the_persistence_id = () =>
            MessageSender.SentMessages.ExcludeAcknowledgements().First()
                .ShouldHaveCorrectPersistenceId(ReceiverName, PersistenceUseType.PointToPointSend);

        It should_set_original_persistence_id_to_the_persistence_id_of_the_message_with_the_persistence_id = () =>
           MessageSender.SentMessages.ExcludeAcknowledgements().First().GetSourcePersistenceId()
               .ShouldEqual(MessageSender.SentMessages.ExcludeAcknowledgements().First().GetPersistenceId());

        It should_send_a_message_with_the_correct_content = () => 
            MessageSender.SentMessages.ExcludeAcknowledgements().First().DeserialiseTo<int>().ShouldEqual(message);

        It should_mark_the_time_the_message_is_sent = () => 
            MessageSender.SentMessages.ExcludeAcknowledgements().First()
                .GetLastTimeSent().ShouldBeGreaterThan(DateTime.MinValue);

        It should_mark_the_amount_of_times_the_message_has_been_sent = () => 
            MessageSender.SentMessages.ExcludeAcknowledgements().First().GetAmountSent().ShouldEqual(1);

        It should_mark_the_message_with_the_sequence = () =>
            MessageSender.SentMessages.ExcludeAcknowledgements().First().GetSequence().ShouldEqual(1);

        It should_start_the_task_repeater = () => TaskRepeater.Started.ShouldBeTrue();
    }
}