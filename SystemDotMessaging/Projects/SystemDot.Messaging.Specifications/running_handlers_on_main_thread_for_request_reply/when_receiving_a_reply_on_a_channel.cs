using System;
using SystemDot.Messaging.Packaging;
using SystemDot.ThreadMashalling;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.running_handlers_on_main_thread_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_reply_on_a_channel_configured_to_handle_on_main_thread : WithMessageConfigurationSubject
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
                    .HandleRepliesOnMainThread()
                .Initialise();

            payload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel(ReceiverChannel)
                .SetToChannel(SenderChannel)
                .SetChannelType(PersistenceUseType.ReplySend)
                .Sequenced();
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_switch_to_the_main_thread_to_handle_the_message = () => MainThreadMarshaller.WasRunThrough.ShouldBeTrue();
    }
}