using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Upcasting;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating_constant
{
    [Subject(SpecificationGroup.Description)]
    public class when_restarting_messaging_on_a_durable_channel_after_sending_a_message_without_receiving_its_acknowledgement
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverAddress = "TestReceiverAddress";
        const Int64 Message = 1;

        static ChangeStore changeStore;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();

            ConfigureAndRegister(changeStore);
            
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForPointToPointSendingTo(ReceiverAddress)
                .WithDurability()
                .RepeatMessages().Every(TimeSpan.FromSeconds(10))
                .Initialise();

            Bus.Send(Message);

            Reset();
            ReInitialise();

            ConfigureAndRegister(changeStore);
            SystemTime.AdvanceTime(TimeSpan.FromDays(1));
        };

        Because of = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForPointToPointSendingTo(ReceiverAddress)
                .WithDurability()
                .RepeatMessages().Every(TimeSpan.FromSeconds(10))
                .Initialise();

        It should_send_the_message_again = () =>
            GetServer().SentMessages.ShouldContain(m => m.DeserialiseTo<Int64>() == Message);
    }
}