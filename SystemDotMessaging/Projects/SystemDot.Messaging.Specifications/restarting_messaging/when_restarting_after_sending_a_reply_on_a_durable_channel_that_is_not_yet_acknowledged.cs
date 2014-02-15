using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Storage.Changes;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.restarting_messaging
{
    [Subject(SpecificationGroup.Description)]
    public class when_restarting_after_sending_a_reply_on_a_durable_channel_that_is_not_yet_acknowledged
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSenderAddress";
        const Int64 Request = 1;
        const Int64 Reply = 2;

        static ChangeStore changeStore;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();

            ConfigureAndRegister(changeStore);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplyReceiving()
                .WithDurability()
                .Initialise();

            GetServer().ReceiveMessage(
                new MessagePayload().MakeSequencedReceivable(
                    Request,
                    SenderAddress,
                    ChannelName,
                    PersistenceUseType.RequestSend));

            Bus.Reply(Reply);

            Reset();
            ReInitialise();

            ConfigureAndRegister(changeStore);
            SystemTime.AdvanceTime(TimeSpan.FromDays(1));
        };

        Because of = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplyReceiving()
                .WithDurability()
                .Initialise();

        It should_send_the_message_again = () => GetServer().SentMessages.Should().Contain(m => m.DeserialiseTo<Int64>() == Reply);
    }
}