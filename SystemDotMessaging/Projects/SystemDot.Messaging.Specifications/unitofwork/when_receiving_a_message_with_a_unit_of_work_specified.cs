using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.unitofwork
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_with_a_unit_of_work_specified : WithMessageConfigurationSubject
    {
        const int Message = 1;
        const string ReceiverAddress = "ReceiverAddress";

        static MessagePayload payload;
        static TestMessageHandler<int> handler;
        static TestUnitOfWork unitOfWork;

        Establish context = () =>
        {
            unitOfWork = new TestUnitOfWork();
            ConfigureAndRegister<TestUnitOfWorkFactory>(new TestUnitOfWorkFactory(unitOfWork));

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress)
                    .ForPointToPointReceiving()
                        .WithUnitOfWork<TestUnitOfWorkFactory>()
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            payload = new MessagePayload().MakeSequencedReceivable(
                Message,
                "SenderAddress",
                ReceiverAddress,
                PersistenceUseType.PointToPointReceive);
        };

        Because of = () => Server.ReceiveMessage(payload);

        It should_begin_the_unit_of_work = () => unitOfWork.HasBegun().ShouldBeTrue();

        It should_process_the_message = () => handler.LastHandledMessage.ShouldEqual(Message);

        It should_end_the_unit_of_work = () => unitOfWork.HasEnded().ShouldBeTrue();
    }
}