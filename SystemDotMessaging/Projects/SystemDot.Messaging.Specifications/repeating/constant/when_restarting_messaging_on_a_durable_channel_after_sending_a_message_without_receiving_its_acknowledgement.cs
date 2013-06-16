using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using SystemDot.Specifications;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating.constant
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

            ConfigureAndRegister<IChangeStore>(changeStore);
            ConfigureAndRegister<ITaskRepeater>(new TestTaskRepeater());

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForPointToPointSendingTo(ReceiverAddress)
                .WithDurability()
                .WithMessageRepeating(RepeatMessages.Every(TimeSpan.FromSeconds(10)))
                .Initialise();

            Bus.Send(Message);

            Reset();
            ReInitialise();

            ConfigureAndRegister<IChangeStore>(changeStore);
            ConfigureAndRegister<ITaskRepeater>(new TestTaskRepeater());
            ConfigureAndRegister<ISystemTime>(new TestSystemTime(DateTime.Now.AddDays(1)));
        };

        Because of = () =>
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForPointToPointSendingTo(ReceiverAddress)
                .WithDurability()
                .WithMessageRepeating(RepeatMessages.Every(TimeSpan.FromSeconds(10)))
                .Initialise();

        It should_send_the_message_again = () =>
            Server.SentMessages.ShouldContain(m => m.DeserialiseTo<Int64>() == Message);
    }
}