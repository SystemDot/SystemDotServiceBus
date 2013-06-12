using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.client
{
    [Subject(SpecificationGroup.Description)]
    public class when_long_polling_and_messages_only_exist_on_the_server_for_another_client : WithHttpConfigurationSubject
    {
        const string ReceiverName = "ReceiverName";
        const string SenderName = "SenderName";
        const string Server = "Server";
        const string Proxy = "Proxy";
        const string RemoteProxyInstance = "RemoteProxyInstance";

        static TestTaskStarter taskStarter; 
        static MessagePayload messagePayload;
        static TestMessageHandler<int> handler;

        Establish context = () =>
        {
            WebRequestor.ExpectAddress("DifferentServer", null);

            taskStarter = new TestTaskStarter(1);
            taskStarter.Pause(); 
            ConfigureAndRegister<ITaskStarter>(taskStarter);

            Messaging.Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServerUsingAProxy(Server, Proxy)
                .OpenChannel(ReceiverName)
                .ForPointToPointReceiving()
                .Initialise();

            messagePayload =
                 new MessagePayload().MakeSequencedReceivable(
                 1, 
                 SenderName, 
                 "DifferentReceiver", 
                 Server,
                 RemoteProxyInstance,
                 PersistenceUseType.PointToPointSend);

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () =>
        {
            WebRequestor.AddMessages(messagePayload);
            taskStarter.UnPause();
        };

        It should_not_receive_any_messages = () => handler.LastHandledMessage.ShouldEqual(0);
    }
}