using System.Linq;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.request_reply.batching
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_requests_inside_a_batch : WithMessageConfigurationSubject
    {
        const int Message1 = 1;
        const int Message2 = 2;
        
        Establish context = () => Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel("SenderAddress").ForRequestReplySendingTo("ReceiverAddress")
            .Initialise();

        Because of = () =>
        {
            using (Batch batch = Bus.BatchSend())
            {
                Bus.Send(Message1);
                Bus.Send(Message2);

                batch.Complete();
            }
        };

        It should_send_a_batch_containing_both_messages = () =>
            Server.SentMessages.Single().DeserialiseTo<BatchMessage>().Messages.ShouldContain(Message1, Message2);
    }
}