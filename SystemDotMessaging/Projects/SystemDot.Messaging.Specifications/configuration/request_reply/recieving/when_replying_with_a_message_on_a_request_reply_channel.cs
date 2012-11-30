using System;
using System.Linq;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;
using SystemDot.Messaging.Specifications.configuration.publishing;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject("Request reply configuration")]
    public class when_replying_with_a_message_on_a_request_reply_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";

        static InMemoryChangeStore persistentMemoryChangeStore;
        static IBus bus;
        static int message;

        Establish context = () =>
        {
            persistentMemoryChangeStore = new InMemoryChangeStore(new PlatformAgnosticSerialiser());
            ConfigureAndRegister<MessageCacheFactory>(new MessageCacheFactory(persistentMemoryChangeStore));

            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplyRecieving()
                .Initialise();

            MessageReciever.RecieveMessage(CreateReceiveablePayload(1, SenderChannelName, ChannelName, PersistenceUseType.RequestSend));

            message = 1;
        };

        Because of = () => bus.Reply(message);

        It should_send_a_message_with_the_correct_to_address = () =>
            MessageSender.SentMessages.ExcludeAcknowledgements().First().GetToAddress().ShouldEqual(BuildAddress(SenderChannelName));

        It should_send_a_message_with_the_correct_from_address = () =>
            MessageSender.SentMessages.ExcludeAcknowledgements().First().GetFromAddress().ShouldEqual(BuildAddress(ChannelName));

        It should_send_a_message_with_the_correct_content = () =>
            Deserialise<int>(MessageSender.SentMessages.ExcludeAcknowledgements().First().GetBody()).ShouldEqual(message);

        It should_mark_the_message_with_the_persistence_id = () =>
            MessageSender.SentMessages.ExcludeAcknowledgements().First().GetPersistenceId()
                .ShouldEqual(new MessagePersistenceId(
                    MessageSender.SentMessages.ExcludeAcknowledgements().First().Id,
                    BuildAddress(SenderChannelName),
                    PersistenceUseType.ReplySend));

        It should_set_original_persistence_id_to_the_persistence_id_of_the_message_with_the_persistence_id = () =>
           MessageSender.SentMessages.ExcludeAcknowledgements().First().GetSourcePersistenceId()
               .ShouldEqual(MessageSender.SentMessages.ExcludeAcknowledgements().First().GetPersistenceId());

        It should_mark_the_time_the_message_is_sent = () =>
            MessageSender.SentMessages.ExcludeAcknowledgements().First().GetLastTimeSent().ShouldBeGreaterThan(DateTime.MinValue);

        It should_mark_the_amount_of_times_the_message_has_been_sent = () =>
            MessageSender.SentMessages.ExcludeAcknowledgements().First().GetAmountSent().ShouldEqual(1);

        It should_mark_the_message_with_the_sequence = () =>
            MessageSender.SentMessages.ExcludeAcknowledgements().First().GetSequence().ShouldEqual(1);

        It should_not_persist_the_message = () =>
            persistentMemoryChangeStore
                .GetMessages(PersistenceUseType.ReplySend, BuildAddress(SenderChannelName))
                .ShouldBeEmpty();

        It should_start_the_task_repeater = () => The<ITaskRepeater>().WasToldTo(r => r.Start());
    }
}