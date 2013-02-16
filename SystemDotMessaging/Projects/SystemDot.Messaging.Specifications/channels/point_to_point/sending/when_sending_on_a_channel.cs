using System;
using System.Linq;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Specifications.channels.publishing;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.sending
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

        It should_send_a_message_with_the_correct_to_address_channel_name = () =>
            Server.SentMessages.First().GetToAddress()
                .Channel.ShouldEqual(ReceiverName);

        It should_send_a_message_with_the_to_address_server_path_not_set = () =>
            Server.SentMessages.First().GetToAddress().ServerPath.ShouldEqual(ServerPath.None);

        It should_send_a_message_with_the_correct_from_address = () =>
            Server.SentMessages.First().GetFromAddress().ShouldEqual(BuildAddress(ChannelName));

        It should_mark_the_message_with_the_persistence_id = () =>
            Server.SentMessages.ExcludeAcknowledgements().First()
                .ShouldHaveCorrectPersistenceId(ChannelName, PersistenceUseType.PointToPointSend);

        It should_set_original_persistence_id_to_the_persistence_id_of_the_message_with_the_persistence_id = () =>
           Server.SentMessages.ExcludeAcknowledgements().First().GetSourcePersistenceId()
               .ShouldEqual(Server.SentMessages.ExcludeAcknowledgements().First().GetPersistenceId());

        It should_send_a_message_with_the_correct_content = () => 
            Server.SentMessages.ExcludeAcknowledgements().First().DeserialiseTo<int>().ShouldEqual(message);

        It should_mark_the_time_the_message_is_sent = () => 
            Server.SentMessages.ExcludeAcknowledgements().First()
                .GetLastTimeSent().ShouldBeGreaterThan(DateTime.MinValue);

        It should_mark_the_amount_of_times_the_message_has_been_sent = () => 
            Server.SentMessages.ExcludeAcknowledgements().First().GetAmountSent().ShouldEqual(1);

        It should_mark_the_message_with_the_sequence = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().GetSequence().ShouldEqual(1);

        It should_start_the_task_repeater = () => TaskRepeater.Started.ShouldBeTrue();
    }
}