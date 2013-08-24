using System;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating_escalating
{
    [Subject(SpecificationGroup.Description)]
    public class when_restarting_messaging_on_a_durable_channel_after_sending_a_message_without_receiving_its_acknowledgement
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverAddress = "TestReceiverAddress";
        const Int64 Message = 1;

        static IChangeStore changeStore;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore(new JsonSerialiser());

            ConfigureAndRegister(changeStore);
            
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForPointToPointSendingTo(ReceiverAddress)
                .WithDurability()
                .Initialise();

            Bus.Send(Message);

            Reset();
            ReInitialise();

            ConfigureAndRegister(changeStore);
            SystemTime.AdvanceTime(TimeSpan.FromDays(1));
        };

        Because of = () =>
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForPointToPointSendingTo(ReceiverAddress)
                .WithDurability()
                .Initialise();

        It should_send_the_message_again = () => GetServer().SentMessages.ShouldContain(m => m.DeserialiseTo<Int64>() == Message);
    }
}