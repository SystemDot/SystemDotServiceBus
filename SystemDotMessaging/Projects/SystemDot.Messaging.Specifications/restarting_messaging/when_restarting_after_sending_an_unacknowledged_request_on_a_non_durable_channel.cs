using System;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.restarting_messaging
{
    [Subject(SpecificationGroup.Description)]
    public class when_restarting_after_sending_an_unacknowledged_request_on_a_non_durable_channel
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverAddress = "TestReceiverAddress";
        const int Request = 1;

        static ChangeStore changeStore;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();

            ConfigureAndRegister(changeStore);
            
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplySendingTo(ReceiverAddress)
                .Initialise();

            Bus.Send(Request);

            Reset();
            ReInitialise();

            ConfigureAndRegister(changeStore);
            SystemTime.AdvanceTime(TimeSpan.FromDays(1));
        };

        Because of = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplySendingTo(ReceiverAddress)
                .Initialise();

        It should_not_send_the_message_again = () =>
            GetServer().SentMessages.ShouldNotContain(m => m.DeserialiseTo<int>() == Request);
    }
}