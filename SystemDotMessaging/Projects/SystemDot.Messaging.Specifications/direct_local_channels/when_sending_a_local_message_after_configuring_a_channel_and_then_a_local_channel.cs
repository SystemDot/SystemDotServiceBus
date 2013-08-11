using SystemDot.Messaging.Handling;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.direct_local_channels
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_local_message_after_configuring_a_channel_and_then_a_local_channel 
        : WithMessageConfigurationSubject
    {
        static int message;
        static TestMessageHandler<int> handler;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("Channel").ForRequestReplyReceiving()
                .OpenDirectChannel()
                .Initialise();

            message = 1;

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () => Bus.SendDirect(message);

        It should_send_the_message_to_any_handlers_registered_for_that_message = () => 
            handler.LastHandledMessage.ShouldEqual(message);
    }
}