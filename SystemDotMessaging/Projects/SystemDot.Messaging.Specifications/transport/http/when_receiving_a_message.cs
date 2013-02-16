using SystemDot.Http.Builders;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.Http.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message : WithServerConfigurationSubject
    {
        const int Message = 1;
        const string ServerInstance = "ServerInstance";
        const string ReceiverAddress = "ReceiverAddress";

        static TestMessageHandler<int> handler;
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            ConfigureAndRegister<IHttpServerBuilder>(new TestHttpServerBuilder());

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ServerInstance)
                .OpenChannel(ReceiverAddress).ForPointToPointReceiving()
                .Initialise();

            messagePayload =
                new MessagePayload().MakeReceiveable(
                    Message,
                    "SenderAddress",
                    ReceiverAddress,
                    ServerInstance,
                    ServerInstance,
                    PersistenceUseType.PointToPointSend);

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () => SendMessagesToServer(messagePayload);

        It should_receive_the_message_down_the_channel = () => handler.LastHandledMessage.ShouldEqual(Message);
    }
}