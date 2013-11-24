using System;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Upcasting;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.restarting_messaging
{
    [Subject(SpecificationGroup.Description)]
    public class when_restarting_messaging_after_sending_a_request_on_a_durable_channel_that_is_not_yet_acknowledged 
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverAddress = "TestReceiverAddress";
        const Int64 Request = 1;
        
        static ChangeStore changeStore;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore(new JsonSerialiser(), new ChangeUpcasterRunner());

            ConfigureAndRegister(changeStore);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplySendingTo(ReceiverAddress)
                .WithDurability()
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
                .WithDurability()
                .Initialise();

        It should_send_the_message_again = () => GetServer().SentMessages.ShouldContain(m => m.DeserialiseTo<Int64>() == Request);
    }
}