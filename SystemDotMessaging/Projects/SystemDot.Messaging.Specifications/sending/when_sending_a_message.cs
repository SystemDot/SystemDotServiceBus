using System;
using System.Linq;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Specifications.publishing;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.sending
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message : WithMessageConfigurationSubject
    {
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";

        static MessageAddedToCache messageAddedToCacheEvent;

        static Int64 message;
        
        Establish context = () =>
        {
            Messenger.Register<MessageAddedToCache>(e => messageAddedToCacheEvent = e);

            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress).ForPointToPointSendingTo(ReceiverAddress)
                .Initialise();

            message = 1;
        };

        Because of = () => Bus.Send(message);

        It should_notify_that_the_message_was_cached = () =>
            messageAddedToCacheEvent.ShouldMatch(m =>
                m.CacheAddress == BuildAddress(SenderAddress)
                && m.UseType == PersistenceUseType.PointToPointSend
                && m.Message == Server.SentMessages.First());

        It should_send_a_message_with_the_correct_to_address_channel_name = () =>
            Server.SentMessages.First().GetToAddress()
                .Channel.ShouldEqual(ReceiverAddress);

        It should_send_a_message_with_the_to_address_server_path_not_set = () =>
            Server.SentMessages.First().GetToAddress().ServerPath.ShouldEqual(ServerPath.None);

        It should_send_a_message_with_the_correct_from_address = () =>
            Server.SentMessages.First().GetFromAddress().ShouldEqual(BuildAddress(SenderAddress));

        It should_mark_the_message_with_the_persistence_id = () =>
            Server.SentMessages.ExcludeAcknowledgements().First()
                .ShouldHaveCorrectPersistenceId(SenderAddress, PersistenceUseType.PointToPointSend);

        It should_set_original_persistence_id_to_the_persistence_id_of_the_message_with_the_persistence_id = () =>
           Server.SentMessages.ExcludeAcknowledgements().First().GetSourcePersistenceId()
               .ShouldEqual(Server.SentMessages.ExcludeAcknowledgements().First().GetPersistenceId());

        It should_send_a_message_with_the_correct_content = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().DeserialiseTo<Int64>().ShouldEqual(message);

        It should_mark_the_time_the_message_is_sent = () => 
            Server.SentMessages.ExcludeAcknowledgements().First()
                .GetLastTimeSent().ShouldBeGreaterThan(DateTime.MinValue);

        It should_mark_the_amount_of_times_the_message_has_been_sent = () => 
            Server.SentMessages.ExcludeAcknowledgements().First().GetAmountSent().ShouldEqual(1);

        It should_start_the_task_repeater = () => TaskRepeater.Started.ShouldBeTrue();
    }
}