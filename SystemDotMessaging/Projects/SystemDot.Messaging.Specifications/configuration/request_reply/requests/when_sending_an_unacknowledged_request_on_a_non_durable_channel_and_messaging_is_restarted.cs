using System;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using SystemDot.Specifications;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.requests
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_an_unacknowledged_request_on_a_non_durable_channel_and_messaging_is_restarted
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverAddress = "TestReceiverAddress";
        const int Request = 1;

        static IChangeStore changeStore;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore(new PlatformAgnosticSerialiser());

            ConfigureAndRegister<IChangeStore>(changeStore);
            ConfigureAndRegister<ITaskRepeater>(new TestTaskRepeater());

            IBus bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplySendingTo(ReceiverAddress)
                .Initialise();

            bus.Send(Request);

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
                .ForRequestReplySendingTo(ReceiverAddress)
                .Initialise();

        It should_not_send_the_message_again = () =>
            MessageSender.SentMessages.ShouldNotContain(m => m.DeserialiseTo<int>() == Request);
    }
}