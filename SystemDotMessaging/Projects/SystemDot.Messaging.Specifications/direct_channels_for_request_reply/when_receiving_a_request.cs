using System;
using SystemDot.Messaging.Direct;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;using FluentAssertions;

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
                .SetFromChannel("Sender")
                .SetToChannel(Receiver);

            payload.SetIsDirectChannelMessage();

            handler = new TestMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenDirectChannel(Receiver).ForRequestReplyReceiving()
                .RegisterHandlers(h => h.RegisterHandler(handler))
                .Initialise();
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.LastHandledMessage.ShouldBeEquivalentTo(Message);
    }
}