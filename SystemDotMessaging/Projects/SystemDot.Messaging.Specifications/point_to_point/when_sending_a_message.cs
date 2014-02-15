using System;
using System.Linq;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Simple;
using SystemDot.Messaging.Specifications.publishing;
using FluentAssertions;
using Machine.Specifications;using FluentAssertions;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.point_to_point
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
            Messenger.RegisterHandler<MessageAddedToCache>(e => messageAddedToCacheEvent = e);

            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress).ForPointToPointSendingTo(ReceiverAddress)
                .Initialise();

            message = 1;
        };

        Because of = () => Bus.Send(message);

        It should_notify_that_the_message_was_cached = () =>
            messageAddedToCacheEvent.Should().Match<MessageAddedToCache>(m =>
                m.CacheAddress == BuildAddress(SenderAddress)
                && m.UseType == PersistenceUseType.PointToPointSend
                && m.Message == GetServer().SentMessages.First());

        It should_send_a_message_with_the_correct_to_address_channel_name = () =>
            GetServer().SentMessages.First().GetToAddress()
                .Channel.ShouldBeEquivalentTo(ReceiverAddress);

        It should_send_a_message_with_the_to_address_server_path_not_set = () =>
            GetServer().SentMessages.First().GetToAddress().Server.ShouldBeEquivalentTo(MessageServer.None);

        It should_send_a_message_with_the_correct_from_address = () =>
            GetServer().SentMessages.First().GetFromAddress().ShouldBeEquivalentTo(BuildAddress(SenderAddress));

        It should_mark_the_message_with_the_persistence_id = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First()
                .ShouldHaveCorrectPersistenceId(SenderAddress, PersistenceUseType.PointToPointSend);

        It should_set_original_persistence_id_to_the_persistence_id_of_the_message_with_the_persistence_id = () =>
           GetServer().SentMessages.ExcludeAcknowledgements().First().GetSourcePersistenceId()
               .ShouldBeEquivalentTo(GetServer().SentMessages.ExcludeAcknowledgements().First().GetPersistenceId());

        It should_send_a_message_with_the_correct_content = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().DeserialiseTo<Int64>().ShouldBeEquivalentTo(message);

        It should_mark_the_time_the_message_is_sent = () => 
            GetServer().SentMessages.ExcludeAcknowledgements().First()
                .GetLastTimeSent().Should().BeOnOrAfter(DateTime.MinValue);

        It should_mark_the_amount_of_times_the_message_has_been_sent = () => 
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetAmountSent().ShouldBeEquivalentTo(1);

        It should_start_the_task_repeater = () => TaskRepeater.Started.Should().BeTrue();
    }
}