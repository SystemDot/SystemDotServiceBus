using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.receiving_over_http
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message : WithHttpServerConfigurationSubject
    {
        const Int64 Message = 1;
        const string ServerName = "ServerName";
        const string ReceiverChannel = "ReceiverChannel";

        static TestMessageHandler<Int64> handler;
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ServerName)
                .OpenChannel(ReceiverChannel).ForPointToPointReceiving()
                .Initialise();

            messagePayload =
                new MessagePayload().MakeSequencedReceivable(
                    Message,
                    BuildAddress("SenderChannel", ServerName),
                    BuildAddress("ReceiverChannel", ServerName),
                    PersistenceUseType.PointToPointSend);

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlingEndpoint>().RegisterHandler(handler);
        };

        Because of = () => SendMessageToServer(messagePayload);

        It should_receive_the_message_down_the_channel = () => handler.LastHandledMessage.ShouldEqual(Message);
    }
}