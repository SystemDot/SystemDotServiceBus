using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.receiving
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message : WithServerConfigurationSubject
    {
        const Int64 Message = 1;
        const string ServerName = "ServerName";
        const string ReceiverChannel = "ReceiverChannel";

        static TestMessageHandler<Int64> handler;
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            WebRequestor.ExpectAddress(ServerName, Environment.MachineName);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ServerName)
                .OpenChannel(ReceiverChannel).ForPointToPointReceiving()
                .Initialise();

            messagePayload =
                new MessagePayload().MakeSequencedReceivable(
                    Message,
                    BuildAddressWithProxy("SenderChannel", ServerName, ServerName),
                    BuildAddressWithProxy("ReceiverChannel", ServerName, ServerName),
                    PersistenceUseType.PointToPointSend);

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () => SendMessagesToServer(messagePayload);

        It should_receive_the_message_down_the_channel = () => handler.LastHandledMessage.ShouldEqual(Message);
    }
}