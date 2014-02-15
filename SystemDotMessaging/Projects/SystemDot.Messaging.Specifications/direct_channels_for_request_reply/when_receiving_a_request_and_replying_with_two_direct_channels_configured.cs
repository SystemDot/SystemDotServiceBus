using System;
using System.Linq;
using SystemDot.Messaging.Direct;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.direct_channels_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_and_replying_with_two_direct_channels_configured : WithMessageConfigurationSubject
    {
        const string Receiver = "Receiver";
        const string Sender = "Sender";
        const Int64 Reply = 1;

        static TestReplyMessageHandler<long> handler;
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            handler = new TestReplyMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenDirectChannel(Receiver).ForRequestReplyReceiving()
                .OpenDirectChannel("OtherReceiver").ForRequestReplyReceiving()
                .RegisterHandlers(h => h.RegisterHandler(handler))
                .Initialise();

            messagePayload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel(Sender)
                .SetToChannel(Receiver);

            messagePayload.SetIsDirectChannelMessage();
        };

        Because of = () => GetServer().ReceiveMessage(messagePayload);

        It should_send_reply_with_only_the_message_from_the_expected_channel = () => GetServer().ReturnedMessages.Count().ShouldBeEquivalentTo(1);
    }
}