using System;
using System.Linq;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Specifications.channels.publishing;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Serialisation;
using SystemDot.Specifications;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.sequencing
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_on_durable_channel_and_then_restarting_messaging : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverName = "TestReceiver";

        static IBus bus;
        static DateTime originDate;
        static IChangeStore changeStore;
        
        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore(new PlatformAgnosticSerialiser());
            ConfigureAndRegister<IChangeStore>(changeStore);
            
            originDate = DateTime.Now.AddHours(-1);
            ConfigureAndRegister<ISystemTime>(new TestSystemTime(originDate));

            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPointToPointSendingTo(ReceiverName)
                    .WithDurability()
                .Initialise();

            bus.Send(new object());

            ResetIoc();
            Initialise();

            ConfigureAndRegister<IChangeStore>(changeStore);
            ConfigureAndRegister<ISystemTime>(new TestSystemTime(DateTime.Now));

            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPointToPointSendingTo(ReceiverName)
                    .WithDurability()
                .Initialise();
        };

        Because of = () => bus.Send(new object());

        It should_mark_the_message_with_the_initial_sequence_origin_date = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().GetSequenceOriginSetOn().ShouldEqual(originDate);
    }
}