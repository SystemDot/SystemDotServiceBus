using System;
using SystemDot.Messaging.Sequencing;
using SystemDot.Storage.Changes;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.restarting_messaging
{
    [Subject(SpecificationGroup.Description)]
    public class when_restarting_on_a_durable_sequenced_channel_after_a_checkpoint_and_then_sending_a_request
        : WithMessageConfigurationSubject
    {
        const string SenderChannel = "SenderChannel";
        const string ReceiverChannel = "ReceiverChannel";
        const Int64 Message = 1;

        static ChangeStore changeStore;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();
            ConfigureAndRegister<ChangeStore>(changeStore);
            ConfigureAndRegister<ICheckpointStrategy>(new CheckpointAfterOneChangeCheckpointStrategy());

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderChannel)
                .ForRequestReplySendingTo(ReceiverChannel)
                .WithDurability()
                .Sequenced()
                .Initialise();

            Bus.Send(Message);
            GetServer().ReceiveAcknowledgementFor(GetServer().GetOnlyMessage());
            GetServer().SentMessages.Clear();

            Bus.Send(Message);
            GetServer().ReceiveAcknowledgementFor(GetServer().GetOnlyMessage());
            GetServer().SentMessages.Clear();
            
            Reset();
            ReInitialise();

            ConfigureAndRegister<ChangeStore>(changeStore);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderChannel)
                .ForRequestReplySendingTo(ReceiverChannel)
                .WithDurability()
                .Sequenced()
                .Initialise();

            SystemTime.AdvanceTime(TimeSpan.FromDays(1));
        };

        Because of = () => Bus.Send(Message);

        It should_not_reset_the_message_sequence = () => 
            GetServer().GetOnlyMessage().GetSequenceOriginSetOn().Should().NotBe(SystemTime.GetCurrentDate());
    }
}