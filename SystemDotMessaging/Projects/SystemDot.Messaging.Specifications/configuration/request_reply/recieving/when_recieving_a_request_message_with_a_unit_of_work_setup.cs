using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.UnitOfWork;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject("Request reply configuration")]
    public class when_recieving_a_request_message_with_a_unit_of_work_setup : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSenderAddress";

        static MessagePayload payload;
        static TestMessageHandler<int> handler;

        Establish context = () =>
        {
            ConfigureAndRegister<IUnitOfWork>(new TestUnitOfWork());

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplyRecieving()
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            payload = CreateRecieveablePayload(1, SenderAddress, ChannelName, PersistenceUseType.RequestSend);
        };

        Because of = () => MessageReciever.RecieveMessage(payload);

        It should_begin_the_unit_of_work = () =>
            Resolve<IUnitOfWork>()
                .As<TestUnitOfWork>()
                .HasBegun().ShouldBeTrue();
    }
}