using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Specifications.publishing;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.load_balancing_for_request_reply_requests
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_twenty_one_requests_that_are_not_yet_acknowledged : WithMessageConfigurationSubject
    {
        static List<int> messages;
        

        Establish context = () =>
        {
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("SenderAddress")
                .ForRequestReplySendingTo("ReceiverAddress")
                .Initialise();

            messages = Enumerable.Range(1, 21).ToList();
        };

        Because of = () => messages.ForEach(m => Bus.Send(m));

        It should_not_send_the_twenty_first_message = () => Server.SentMessages.ExcludeAcknowledgements().Count.ShouldEqual(20);
    }
}
