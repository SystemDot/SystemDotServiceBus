using System;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.direct_channels_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request : WithMessageConfigurationSubject
    {
        const long Message = 1;
        const string Receiver = "Receiver";

        static MessagePayload payload;
        static TestMessageHandler<Int64> handler;

        Establish context = () =>
        {
            payload = new MessagePayload()
                .SetMessageBody(Message)
                .SetToChannel(Receiver);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenDirectChannel(Receiver).ForRequestReplyReceiving()
                .Initialise();

            handler = new TestMessageHandler<Int64>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.LastHandledMessage.ShouldEqual(Message);
    }
}