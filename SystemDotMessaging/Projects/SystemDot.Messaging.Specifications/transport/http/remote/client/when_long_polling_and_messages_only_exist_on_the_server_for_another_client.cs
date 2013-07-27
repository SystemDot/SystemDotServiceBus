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
        const string ReceiverChannel = "ReceiverChannel";
        const string SenderChannel = "SenderChannel";
        const string ServerName = "ServerName";
        const string ProxyName = "ProxyName";

        static TestTaskStarter taskStarter; 
        static MessagePayload messagePayload;
        static TestMessageHandler<int> handler;

        Establish context = () =>
        {
            taskStarter = new TestTaskStarter(1);
            taskStarter.Pause(); 
            ConfigureAndRegister<ITaskStarter>(taskStarter);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServerUsingAProxy(ServerName, ProxyName)
                .OpenChannel(ReceiverChannel)
                .ForPointToPointReceiving()
                .Initialise();

            messagePayload =
                 new MessagePayload().MakeSequencedReceivable(
                 1,
                 BuildAddressWithProxy(SenderChannel, ServerName, ProxyName),
                 BuildAddressWithProxy("DifferentReceiverChannel", ServerName, ProxyName),
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