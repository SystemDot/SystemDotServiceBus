using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.hooks_for_publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_an_event_on_a_publish_channel_with_a_hook : WithPublisherSubject
    {
        const string ChannelName = "Test";
        const int Message = 1;
        static TestMessageProcessorHook hook;

        Establish context = () =>
        {
            hook = new TestMessageProcessorHook();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForPublishing()
                    .WithHook(hook)
                .Initialise();
        };

        Because of = () => Bus.Publish(Message);

        It should_run_the_message_through_the_hook = () => hook.Message.ShouldEqual(Message);
    }
}