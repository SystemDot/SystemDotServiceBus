using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.batching
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_messages_inside_a_batch_without_completing_the_batch : WithMessageConfigurationSubject
    {
        const int Message = 1;
        
        Establish context = () => Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel("SenderAddress").ForPointToPointSendingTo("ReceiverAddress")
            .Initialise();

        Because of = () =>
        {
            using (Bus.BatchSend())
            {
                Bus.Send(Message);
            }
        };

        It should_not_send_a_batch_containing_messages = () =>
            Server.SentMessages.ShouldBeEmpty();
    }
}