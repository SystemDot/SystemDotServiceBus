using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.direct_channels_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_from_a_different_channel : WithMessageConfigurationSubject
    {
        const long Message = 1;
        const string Receiver = "Receiver";

        static MessagePayload payload;
        static TestMessageHandler<long> handler;

        Establish context = () =>
        {
            payload = new MessagePayload()
                .SetMessageBody(Message)
                .SetToChannel("OtherReceiver");

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenDirectChannel(Receiver).ForRequestReplyReceiving()
                .Initialise();

            handler = new TestMessageHandler<long>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.LastHandledMessage.ShouldEqual(0);
    }
}