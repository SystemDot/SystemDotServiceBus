using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing;
using SystemDot.Messaging.Repeating;
using SystemDot.Parallelism;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.repeating_constant_for_subscribers
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_an_event_with_a_constant_time_repeat_on_the_channel_and_that_time_has_passed : WithHttpServerConfigurationSubject
    {
        const string PublisherChannel = "PublisherChannel";
        const string PublisherServer = "PublisherServer";
        const int Event = 1;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(PublisherServer)
                .OpenChannel(PublisherChannel)
                .ForPublishing()
                .Initialise();

            MessagePayload subscriptionRequest = new MessagePayload()
                .SetToChannel(PublisherChannel)
                .SetToServer(PublisherServer)
                .SetFromChannel("SubscriberChannel")
                .SetFromServer("SubscriberServer")
                .AsSubscriptionRequest();

            subscriptionRequest.GetSubscriptionRequestSchema().RepeatStrategy = new ConstantTimeRepeatStrategy { RepeatEvery = TimeSpan.FromSeconds(1) };

            SendMessageToServer(subscriptionRequest);

            Bus.Publish(Event);

            WebRequestor.RequestsMade.Clear();

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(1));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_repeat_the_message = () => WebRequestor.RequestsMade.Count.ShouldBeEquivalentTo(1);
    }
}