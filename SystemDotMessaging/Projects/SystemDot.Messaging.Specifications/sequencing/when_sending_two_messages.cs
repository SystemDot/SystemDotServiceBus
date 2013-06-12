using System.Linq;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.sequencing
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_two_messages : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverName = "TestReceiver";
        
        
        const int Message1 = 1;
        const int Message2 = 2;
        
        Establish context = () =>
        {    
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPointToPointSendingTo(ReceiverName)
                .Initialise();
        };

        Because of = () =>
        {
            Bus.Send(Message1);
            Bus.Send(Message2);
        };

        It should_mark_the_last_message_with_a_first_sequence_of_the_lowest_sequence_in_the_cache = () =>
            Server.SentMessages.ExcludeAcknowledgements().Last().GetFirstSequence().ShouldEqual(1);
    }
}