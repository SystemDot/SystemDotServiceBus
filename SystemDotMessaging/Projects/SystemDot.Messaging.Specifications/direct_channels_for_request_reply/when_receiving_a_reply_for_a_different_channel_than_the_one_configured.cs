using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.direct_channels_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_reply_for_a_different_channel_than_the_one_configured : WithMessageConfigurationSubject
    {
        const long Message = 1;

        static TestMessageHandler<long> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenDirectChannel("Sender").ForRequestReplySendingTo("Receiver")
                .RegisterHandlers(h => h.RegisterHandler(handler))
                .Initialise();

            GetServer().ReceiveMessage(new MessagePayload().SetMessageBody(Message).SetToChannel("OtherSender"));
        };

        Because of = () => Bus.Send(Message);

        It should_not_push_the_reply_to_any_handlers = () => handler.HandledMessages.Count.ShouldEqual(0);
    }
}