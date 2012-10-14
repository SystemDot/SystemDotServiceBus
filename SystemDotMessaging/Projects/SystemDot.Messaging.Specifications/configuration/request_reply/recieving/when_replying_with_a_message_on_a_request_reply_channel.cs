using System;
using System.Linq;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject("Request reply configuration")]
    public class when_replying_with_a_message_on_a_request_reply_channel : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";

        static IBus bus;
        static int message;
        static TestPersistence persistence;

        Establish context = () =>
        {
            var persistenceFactory = new TestPersistenceFactory();
            ConfigureAndRegister<IPersistenceFactory>(persistenceFactory);

            persistence = new TestPersistence();
            persistenceFactory.AddPersistence(PersistenceUseType.RequestReceive, new TestPersistence());
            persistenceFactory.AddPersistence(PersistenceUseType.ReplySend, persistence);

            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplyRecieving()
                .Initialise();

            Resolve<ReplyAddressLookup>().SetCurrentRecieverAddress(BuildAddress(ChannelName));
            Resolve<ReplyAddressLookup>().SetCurrentSenderAddress(BuildAddress(SenderChannelName));

            message = 1;
        };

        Because of = () => bus.Reply(message);

        It should_send_a_message_with_the_correct_to_address = () =>
            MessageSender.SentMessages.First().GetToAddress().ShouldEqual(BuildAddress(SenderChannelName));

        It should_send_a_message_with_the_correct_from_address = () =>
            MessageSender.SentMessages.First().GetFromAddress().ShouldEqual(BuildAddress(ChannelName));

        It should_send_a_message_with_the_correct_content = () =>
            Deserialise<int>(MessageSender.SentMessages.First().GetBody()).ShouldEqual(message);

        It should_mark_the_time_the_message_is_sent = () =>
            MessageSender.SentMessages.First().GetLastTimeSent().ShouldBeGreaterThan(DateTime.MinValue);

        It should_mark_the_amount_of_times_the_message_has_been_sent = () =>
            MessageSender.SentMessages.First().GetAmountSent().ShouldEqual(1);

        It should_mark_the_message_with_the_sequence = () =>
            MessageSender.SentMessages.First().GetSequence().ShouldEqual(1);

        It should_not_persist_the_message = () => persistence.GetMessages().ShouldBeEmpty();

        It should_start_the_task_repeater = () => The<ITaskRepeater>().WasToldTo(r => r.Start());
    }
}