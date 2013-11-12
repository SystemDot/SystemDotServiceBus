using System;
using SystemDot.Messaging.Direct;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.direct_channels_for_request_reply_over_http
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request : WithHttpServerConfigurationSubject
    {
        const long Message = 1;
        const string ReceiverChannel = "ReceiverChannel";
        const string ReceiverServer = "ReceiverServer";

        static MessagePayload payload;
        static TestMessageHandler<Int64> handler;

        Establish context = () =>
        {
            payload = new MessagePayload()
                .SetMessageBody(Message)
                .SetFromChannel("Sender")
                .SetFromServer("SenderServer")
                .SetToChannel(ReceiverChannel)
                .SetToServer(ReceiverServer);

            payload.SetIsDirectChannelMessage();

            handler = new TestMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ReceiverServer)
                .OpenDirectChannel(ReceiverChannel).ForRequestReplyReceiving()
                .RegisterHandlers(h => h.RegisterHandler(handler))
                .Initialise();
        };

        Because of = () => SendMessageToServer(payload);

        It should_push_the_message_to_any_registered_handlers = () => handler.LastHandledMessage.ShouldEqual(Message);
    }
}