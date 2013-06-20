using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.unitofwork.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_with_a_unit_of_work_setup : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSenderAddress";

        static MessagePayload payload;
        static TestMessageHandler<int> handler;
        static TestUnitOfWork unitOfWork;

        Establish context = () =>
        {
            unitOfWork = new TestUnitOfWork();
            ConfigureAndRegister<TestUnitOfWorkFactory>(new TestUnitOfWorkFactory(unitOfWork));
            
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForRequestReplyRecieving()
                    .WithUnitOfWork<TestUnitOfWorkFactory>()
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            payload = new MessagePayload().MakeSequencedReceivable(
                1, 
                SenderAddress, 
                ChannelName, 
                PersistenceUseType.RequestSend);
        };

        Because of = () => Server.ReceiveMessage(payload);

        It should_begin_the_unit_of_work = () => unitOfWork.HasBegun().ShouldBeTrue();
    }
}