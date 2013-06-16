using System;
using System.Linq;
using SystemDot.Messaging.Batching;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.batching
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_messages_inside_a_batch : WithMessageConfigurationSubject
    {
        const Int64 Message1 = 1;
        const Int64 Message2 = 2;

        Establish context = () => Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel("SenderAddress").ForPointToPointSendingTo("ReceiverAddress")
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