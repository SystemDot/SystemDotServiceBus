using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.restarting_messaging
{
    [Subject(SpecificationGroup.Description)]
    public class when_restarting_after_sending_a_message_on_a_non_durable_channel_that_is_not_yet_acknowledged
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSenderAddress";
        const int Request = 1;
        const int Reply = 2;

        static ChangeStore changeStore;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();

            ConfigureAndRegister(changeStore);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplyReceiving()
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
                .Initialise();

        It should_not_send_the_message_again = () => GetServer().SentMessages.ShouldNotContain(m => m.DeserialiseTo<int>() == Reply);
    }
}