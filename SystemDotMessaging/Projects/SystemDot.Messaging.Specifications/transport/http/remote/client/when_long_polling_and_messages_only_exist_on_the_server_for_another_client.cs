using SystemDot.Http;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.configuration;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.client
{
    [Subject(SpecificationGroup.Description)]
    public class when_long_polling_and_messages_only_exist_on_the_server_for_another_client : WithConfigurationSubject
    {
        const string ReceiverName = "ReceiverName";
        const string SenderName = "SenderName";

        static TestTaskStarter taskStarter; 
        static MessagePayload messagePayload;
        static TestWebRequestor requestor;
        static TestMessageHandler<int> handler;

        Establish context = () =>
        {
            requestor = new TestWebRequestor(new PlatformAgnosticSerialiser(), new FixedPortAddress("DifferentServer"));
            ConfigureAndRegister<IWebRequestor>(requestor);

            taskStarter = new TestTaskStarter(1);
            taskStarter.Pause(); 
            ConfigureAndRegister<ITaskStarter>(taskStarter);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsARemoteClientOf(MessageServer.Local())
                .OpenChannel(ReceiverName)
                .ForPointToPointReceiving()
                .Initialise();

            messagePayload =
                 new MessagePayload()
                    .MakeReceiveable(1, SenderName, "DifferentReceiver", PersistenceUseType.PointToPointSend);

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () =>
        {
            requestor.AddMessages(messagePayload);
            taskStarter.UnPause();
        };

        It should_not_receive_any_messages = () => handler.LastHandledMessage.ShouldEqual(0);
    }
}