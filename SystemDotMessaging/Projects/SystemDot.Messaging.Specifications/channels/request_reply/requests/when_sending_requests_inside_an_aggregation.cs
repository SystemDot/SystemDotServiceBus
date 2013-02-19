using System.Linq;
using SystemDot.Messaging.Aggregation;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.request_reply.requests
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_requests_inside_an_aggregation : WithMessageConfigurationSubject
    {
        const int Message1 = 1;
        const int Message2 = 2;
        
        static IBus bus;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("SenderAddress").ForRequestReplySendingTo("ReceiverAddress")
                .Initialise();
        };

        Because of = () =>
        {
            using (bus.Aggregate())
            {
                bus.Send(Message1);
                bus.Send(Message2);
            }
        };

        It should_send_an_aggregated_package_containing_both_messages = () =>
            Server.SentMessages.Single().DeserialiseTo<AggregateMessage>().Messages.ShouldContain(Message1, Message2);
    }
}