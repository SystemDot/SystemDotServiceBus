using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.running_handlers_on_main_thread_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_reply_on_a_channel_not_configured_to_handle_on_main_thread : WithMessageConfigurationSubject
    {
        const string SenderChannel = "SenderChannel";
        const string ReceiverChannel = "ReceiverChannel";

        static MessagePayload payload;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderChannel)
                .ForRequestReplySendingTo(ReceiverChannel)
                .Initialise();

            payload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel(ReceiverChannel)
                .SetToChannel(SenderChannel)
                .SetChannelType(PersistenceUseType.ReplySend)
                .Sequenced();
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_not_switch_to_the_main_thread_to_handle_the_message = () => MainThreadMarshaller.WasRunThrough.ShouldBeFalse();
    }
}