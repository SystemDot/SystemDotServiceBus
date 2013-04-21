using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.load_balancing
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_twenty_one_messages_that_are_not_yet_acknowledged : WithMessageConfigurationSubject
    {
        static List<int> messages;
        

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("SenderAddress")
                .ForPointToPointSendingTo("ReceiverAddress")
                .Initialise();

            messages = Enumerable.Range(1, 21).ToList();
        };

        Because of = () => messages.ForEach(m => Bus.Send(m));

        It should_not_send_the_twenty_first_message = () => Server.SentMessages.Count.ShouldEqual(20);
    }
}
