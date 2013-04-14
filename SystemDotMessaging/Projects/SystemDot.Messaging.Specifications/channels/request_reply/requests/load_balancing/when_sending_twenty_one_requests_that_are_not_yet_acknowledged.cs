using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Specifications.channels.publishing;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.request_reply.requests.load_balancing
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_twenty_one_requests_that_are_not_yet_acknowledged : WithMessageConfigurationSubject
    {
        static List<int> messages;
        static IBus bus;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("SenderAddress")
                .ForRequestReplySendingTo("ReceiverAddress")
                .Initialise();

            messages = Enumerable.Range(1, 21).ToList();
        };

        Because of = () => messages.ForEach(m => bus.Send(m));

        It should_not_send_the_twenty_first_message = () => Server.SentMessages.ExcludeAcknowledgements().Count.ShouldEqual(20);
    }
}
