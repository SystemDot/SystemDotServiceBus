using System;
using System.Linq;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.request_reply.requests
{
    [Subject(replies.SpecificationGroup.Description)]
    public class when_sending_a_request_down_a_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";
        
        static IBus bus;
        static int message;
        
        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForRequestReplySendingTo(RecieverAddress)
                .Initialise();

            message = 1;
        };

        Because of = () => bus.Send(message);

        It should_send_a_message_with_the_correct_to_address = () =>
            Server.SentMessages.First().GetToAddress().ShouldEqual(BuildAddress(RecieverAddress));

        It should_send_a_message_with_the_correct_from_address = () =>
            Server.SentMessages.First().GetFromAddress().ShouldEqual(BuildAddress(ChannelName));

        It should_send_a_message_with_the_correct_content = () =>
            Server.SentMessages.First().DeserialiseTo<int>().ShouldEqual(message);

        It should_mark_the_message_with_the_persistence_id = () =>
            Server.SentMessages.First()
                .ShouldHaveCorrectPersistenceId(ChannelName, PersistenceUseType.RequestSend);

        It should_set_original_persistence_id_to_the_persistence_id_of_the_message_with_the_persistence_id = () =>
           Server.SentMessages.First().GetSourcePersistenceId()
               .ShouldEqual(Server.SentMessages.First().GetPersistenceId());

        It should_mark_the_message_with_the_time_the_message_is_sent = () =>
            Server.SentMessages.First().GetLastTimeSent().ShouldBeGreaterThan(DateTime.MinValue);

        It should_mark_the_message_with_the_amount_of_times_the_message_has_been_sent = () =>
            Server.SentMessages.First().GetAmountSent().ShouldEqual(1);

        It should_mark_the_message_with_the_sequence = () =>
            Server.SentMessages.First().GetSequence().ShouldEqual(1);

        It should_start_the_task_repeater = () => TaskRepeater.Started.ShouldBeTrue();
    }
}