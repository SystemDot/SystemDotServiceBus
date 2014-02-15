using System;
using System.Linq;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using FluentAssertions;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_on_a_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";
        const Int64 Reply = 1;

        static MessagePayload messagePayload;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplyReceiving()
                .Initialise();

            messagePayload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel(SenderChannelName)
                .SetToChannel(ChannelName)
                .SetChannelType(PersistenceUseType.RequestSend)
                .Sequenced();

            GetServer().ReceiveMessage(messagePayload);
        };

        Because of = () => Bus.Reply(Reply);

        It should_send_a_message_with_the_correct_to_address = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetToAddress().ShouldBeEquivalentTo(messagePayload.GetFromAddress());

        It should_send_a_message_with_the_correct_from_address = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetFromAddress().ShouldBeEquivalentTo(BuildAddress(ChannelName));

        It should_send_a_message_with_the_correct_content = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().DeserialiseTo<Int64>().ShouldBeEquivalentTo(Reply);

        It should_mark_the_message_with_the_persistence_id = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First()
                .ShouldHaveCorrectPersistenceId(messagePayload.GetFromAddress(), PersistenceUseType.ReplySend);

        It should_set_original_persistence_id_to_the_persistence_id_of_the_message_with_the_persistence_id = () =>
           GetServer().SentMessages.ExcludeAcknowledgements().First().GetSourcePersistenceId()
               .ShouldBeEquivalentTo(GetServer().SentMessages.ExcludeAcknowledgements().First().GetPersistenceId());

        It should_mark_the_time_the_message_is_sent = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetLastTimeSent().Should().BeOnOrAfter(DateTime.MinValue);

        It should_mark_the_amount_of_times_the_message_has_been_sent = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetAmountSent().ShouldBeEquivalentTo(1);

        It should_mark_the_message_with_the_sequence = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetSequence().ShouldBeEquivalentTo(1);

        It should_mark_the_message_with_first_sequence = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetFirstSequence().ShouldBeEquivalentTo(1);

        It should_start_the_task_repeater = () => TaskRepeater.Started.Should().BeTrue();
    }
}