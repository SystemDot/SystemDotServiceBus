using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.filtering_by_name;
using Machine.Specifications;
using SystemDot.Messaging.Direct;

namespace SystemDot.Messaging.Specifications.filtering_by_name_for_direct_channel_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_with_the_wrong_name_down_a_channel : WithMessageConfigurationSubject
    {
        const string Receiver = "Receiver";
        static MessagePayload payload;
        static TestMessageHandler<TestNamePatternMessage> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<TestNamePatternMessage>();
            
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenDirectChannel(Receiver)
                    .ForRequestReplyReceiving()
                        .OnlyForMessages().WithNamePattern("SomethingElse")
                .RegisterHandlers(h => h.RegisterHandler(handler))
                .Initialise();

            payload = new MessagePayload()
                .SetMessageBody(new TestNamePatternMessage())
                .SetFromChannel("Sender")
                .SetToChannel(Receiver);

            payload.SetIsDirectChannelMessage();
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_not_pass_the_message_through = () => handler.HandledMessages.ShouldBeEmpty();
    }
}