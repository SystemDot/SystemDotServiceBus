using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.direct_channels_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_reply_after_sending_a_request_with_a_handler_specified_by_send_direct : WithMessageConfigurationSubject
    {
        const long Message = 1;
        const string Sender = "Sender";

        static TestMessageHandler<long> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenDirectChannel(Sender).ForRequestReplySendingTo("Receiver")
                .Initialise();

            GetServer().ReplyAfterRequestSentWith(new MessagePayload().SetMessageBody(Message).SetToChannel(Sender));
        };

        Because of = () => Bus.SendDirect(Message, handler, exception => { });

        It should_push_the_reply_to_any_registered_handlers = () => handler.LastHandledMessage.ShouldEqual(Message);
    }
}