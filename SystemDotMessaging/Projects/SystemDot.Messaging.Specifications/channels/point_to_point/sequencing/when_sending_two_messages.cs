using System.Linq;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Specifications.channels.publishing;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.sequencing
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_two_messages : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverName = "TestReceiver";
        
        static IBus bus;
        const int Message1 = 1;
        const int Message2 = 2;
        
        Establish context = () =>
        {    
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPointToPointSendingTo(ReceiverName)
                .Initialise();
        };

        Because of = () =>
        {
            bus.Send(Message1);
            bus.Send(Message2);
        };

        It should_mark_the_last_message_with_a_first_sequence_of_the_lowest_sequence_in_the_cache = () =>
            Server.SentMessages.ExcludeAcknowledgements().Last().GetFirstSequence().ShouldEqual(1);
    }
}