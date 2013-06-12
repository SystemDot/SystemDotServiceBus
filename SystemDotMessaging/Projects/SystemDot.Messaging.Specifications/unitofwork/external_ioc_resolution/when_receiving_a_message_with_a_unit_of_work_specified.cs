using SystemDot.Ioc;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.unitofwork.external_ioc_resolution
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
            var container = new IocContainer();
            unitOfWork = new TestUnitOfWork();
            container.RegisterInstance<TestUnitOfWorkFactory>(() => new TestUnitOfWorkFactory(unitOfWork));

            Messaging.Configuration.Configure.Messaging()
                .ResolveReferencesWith(container)
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

        It should_use_the_unit_of_work = () => unitOfWork.HasBegun().ShouldBeTrue();
    }
}