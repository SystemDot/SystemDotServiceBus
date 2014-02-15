using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.unitofwork
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_with_a_unit_of_work_specified_and_the_target_processor_throws_an_exception 
        : WithMessageConfigurationSubject
    {
        const int Message = 1;
        const string ReceiverAddress = "ReceiverAddress";

        static MessagePayload payload;
        static FailingMessageHandler<int> handler;
        static TestUnitOfWork unitOfWork;
        static Exception thrownException;

        Establish context = () =>
        {
            unitOfWork = new TestUnitOfWork();
            ConfigureAndRegister<TestUnitOfWorkFactory>(new TestUnitOfWorkFactory(unitOfWork));

            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress)
                    .ForPointToPointReceiving()
                        .WithUnitOfWork<TestUnitOfWorkFactory>()
                .Initialise();

            handler = new FailingMessageHandler<int>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler);

            payload = new MessagePayload().MakeSequencedReceivable(
                Message,
                "SenderAddress",
                ReceiverAddress,
                PersistenceUseType.PointToPointReceive);
        };

        Because of = () => thrownException = Catch.Exception(() => GetServer().ReceiveMessage(payload));

        It should_pass_the_exception_to_end_of_the_unit_of_work = () => 
            thrownException.Should().BeSameAs(unitOfWork.GetException());
    }
}