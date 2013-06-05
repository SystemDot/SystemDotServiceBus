using System;
using System.Linq;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.replies
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_on_a_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";

        static int message;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplyRecieving()
                .Initialise();

            Server.ReceiveMessage(new MessagePayload().MakeSequencedReceivable(
                1, 
                SenderChannelName, 
                ChannelName, 
                PersistenceUseType.RequestSend));

            message = 1;
        };

        Because of = () => Bus.Reply(message);

        It should_send_a_message_with_the_correct_to_address = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().GetToAddress()
                .ShouldEqual(BuildAddress(SenderChannelName));

        It should_send_a_message_with_the_correct_from_address = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().GetFromAddress()
                .ShouldEqual(BuildAddress(ChannelName));

        It should_send_a_message_with_the_correct_content = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().DeserialiseTo<int>()
                .ShouldEqual(message);

        It should_mark_the_message_with_the_persistence_id = () =>
            Server.SentMessages.ExcludeAcknowledgements().First()
                .ShouldHaveCorrectPersistenceId(SenderChannelName, PersistenceUseType.ReplySend);

        It should_set_original_persistence_id_to_the_persistence_id_of_the_message_with_the_persistence_id = () =>
           Server.SentMessages.ExcludeAcknowledgements().First().GetSourcePersistenceId()
               .ShouldEqual(Server.SentMessages.ExcludeAcknowledgements().First().GetPersistenceId());

        It should_mark_the_time_the_message_is_sent = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().GetLastTimeSent()
                .ShouldBeGreaterThan(DateTime.MinValue);

        It should_mark_the_amount_of_times_the_message_has_been_sent = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().GetAmountSent().ShouldEqual(1);

        It should_mark_the_message_with_the_sequence = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().GetSequence().ShouldEqual(1);

        It should_mark_the_message_with_first_sequence = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().GetFirstSequence().ShouldEqual(1);

        It should_start_the_task_repeater = () => TaskRepeater.Started.ShouldBeTrue();
    }
}