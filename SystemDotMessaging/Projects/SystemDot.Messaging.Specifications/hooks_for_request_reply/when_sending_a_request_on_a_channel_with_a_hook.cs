using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.hooks_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_on_a_channel_with_a_hook : WithMessageConfigurationSubject
    {
        static int message;
        static TestMessageProcessorHook hook;

        Establish context = () =>
        {
            hook = new TestMessageProcessorHook();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("ChannelName")
                    .ForRequestReplySendingTo("RecieverAddress")
                    .WithSendHook(hook)
                .Initialise();

            message = 1;
        };

        Because of = () => Bus.Send(message);

        It should_run_the_message_through_the_hook = () => hook.Message.ShouldEqual(message);
    }
}