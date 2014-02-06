using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.unitofwork
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_with_a_unit_of_work_specified : WithMessageConfigurationSubject
    {
        const int Message = 1;
        const string ReceiverAddress = "ReceiverAddress";

        static MessagePayload payload;
        static TestMessageHandler<Int64> handler;
        static TestUnitOfWork unitOfWork;

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

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler);

            payload = new MessagePayload().MakeSequencedReceivable(
                Message,
                "SenderAddress",
                ReceiverAddress,
                PersistenceUseType.PointToPointReceive);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_begin_the_unit_of_work = () => unitOfWork.HasBegun().ShouldBeTrue();

        It should_process_the_message = () => handler.LastHandledMessage.ShouldEqual(Message);

        It should_end_the_unit_of_work = () => unitOfWork.HasEnded().ShouldBeTrue();
    }
}