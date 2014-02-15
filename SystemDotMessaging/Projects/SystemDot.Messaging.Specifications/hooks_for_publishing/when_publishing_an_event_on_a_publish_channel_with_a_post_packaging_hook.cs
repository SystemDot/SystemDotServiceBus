using System;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.hooks_for_publishing
{
    [Subject(SpecificationGroup.Description)]
    public class when_publishing_an_event_on_a_publish_channel_with_a_post_packaging_hook : WithPublisherSubject
    {
        const string ChannelName = "Test";
        const Int64 Message = 1;
        static TestMessagePayloadProcessorHook hook;

        Establish context = () =>
        {
            hook = new TestMessagePayloadProcessorHook();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForPublishing()
                    .WithHook(hook)
                .Initialise();
        };

        Because of = () => Bus.Publish(Message);

        It should_run_the_message_through_the_hook = () => 
            hook.Message.DeserialiseTo<Int64>().ShouldBeEquivalentTo(Message);
    }
}