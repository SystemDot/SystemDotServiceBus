using System.Linq;
using SystemDot.Messaging.Correlation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.correlation_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_with_correlation_configured : WithMessageConfigurationSubject
    {
        Establish context = () => Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel("SenderChannel")
            .ForRequestReplySendingTo("ReceiverChannel").CorrelateReplyToRequest()
            .Initialise();

        Because of = () => Bus.Send(1);

        It should_send_a_request_with_a_correlation = () =>
            GetServer().SentMessages.First().HasCorrelationId().ShouldBeTrue();
    }
}