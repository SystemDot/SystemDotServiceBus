using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using SystemDot.Specifications;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.replies
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_an_unacknowledged_reply_on_a_durable_channel_and_messaging_is_restarted 
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSenderAddress";
        const int Request = 1;
        const int Reply = 2;

        static IChangeStore changeStore;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore(new PlatformAgnosticSerialiser());

            ConfigureAndRegister<IChangeStore>(changeStore);
            ConfigureAndRegister<ITaskRepeater>(new TestTaskRepeater());

            IBus bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplyRecieving()
                .WithDurability()
                .Initialise();

            MessageServer.ReceiveMessage(
                new MessagePayload().MakeSequencedReceivable(
                    Request,
                    SenderAddress,
                    ChannelName,
                    PersistenceUseType.RequestSend));

            bus.Reply(Reply);

            ResetIoc();
            Initialise();

            ConfigureAndRegister<IChangeStore>(changeStore);
            ConfigureAndRegister<ITaskRepeater>(new TestTaskRepeater());
            ConfigureAndRegister<ISystemTime>(new TestSystemTime(DateTime.Now.AddDays(1)));
        };

        Because of = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplyRecieving()
                .WithDurability()
                .Initialise();

        It should_send_the_message_again = () => 
            MessageServer.SentMessages.ShouldContain(m => m.DeserialiseTo<int>() == Reply);
    }
}