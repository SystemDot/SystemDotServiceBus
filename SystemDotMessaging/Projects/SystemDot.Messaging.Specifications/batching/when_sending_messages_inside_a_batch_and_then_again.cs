using System;
using System.Linq;
using SystemDot.Messaging.Batching;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.batching
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_messages_inside_a_batch_and_then_again : WithMessageConfigurationSubject
    {
        const Int64 Message1 = 1;
        const Int64 Message2 = 2;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("SenderAddress").ForPointToPointSendingTo("ReceiverAddress")
                .Initialise();

            using (Batch batch = Bus.BatchSend())
            {
                Bus.Send(Message1);
                Bus.Send(Message2);

                batch.Complete();
            }
        };

        Because of = () =>
        {
            using (Batch batch = Bus.BatchSend())
            {
                Bus.Send(Message1);
                Bus.Send(Message2);

                batch.Complete();
            }
        };

        It should_send_a_batch_containing_the_first_message_for_the_second_time = () =>
            GetServer().SentMessages.ElementAt(1).DeserialiseTo<BatchMessage>()
                .Messages.Should().Contain(m => m.As<long>() == Message1);
        
        It should_send_a_batch_containing_the_second_message_for_the_second_time = () =>
            GetServer().SentMessages.ElementAt(1).DeserialiseTo<BatchMessage>()
                .Messages.Should().Contain(m => m.As<long>() == Message2);
    }
}