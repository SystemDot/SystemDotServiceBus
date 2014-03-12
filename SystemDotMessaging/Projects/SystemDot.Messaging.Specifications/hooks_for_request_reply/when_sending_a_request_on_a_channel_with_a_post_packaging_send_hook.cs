using System;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.hooks_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_on_a_channel_with_a_post_packaging_send_hook : WithMessageConfigurationSubject
    {
        const Int64 Message = 1;
        static TestMessagePayloadProcessorHook hook;

        Establish context = () =>
        {
            hook = new TestMessagePayloadProcessorHook();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("ChannelName")
                .ForRequestReplySendingTo("RecieverAddress")
                .WithSendHook(hook)
                .Initialise();
        };

        Because of = () => Bus.Send(Message);

        It should_run_the_message_through_the_hook = () => hook.Message.DeserialiseTo<Int64>().ShouldEqual(Message);
    }
}