using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.request_reply.requests
{
    [Subject(replies.SpecificationGroup.Description)]
    public class when_sending_a_request_on_a_channel_with_a_hook : WithMessageConfigurationSubject
    {
        static IBus bus;
        static int message;
        static TestMessageProcessorHook hook;

        Establish context = () =>
        {
            hook = new TestMessageProcessorHook();

            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("ChannelName").ForRequestReplySendingTo("RecieverAddress")
                .WithSendHook(hook)
                .Initialise();

            message = 1;
        };

        Because of = () => bus.Send(message);

        It should_run_the_message_through_the_hook = () => hook.Message.ShouldEqual(message);
    }
}