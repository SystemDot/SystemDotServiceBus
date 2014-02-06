using System;
using SystemDot.Core;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing;
using SystemDot.Messaging.Repeating;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating_constant_for_subscribers
{
    [Subject(SpecificationGroup.Description)]
    public class when_subscribing_to_a_publisher_with_a_constant_time_repeat_strategy_configured : WithHttpConfigurationSubject
    {
        const int RepeatSeconds = 10;

        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport()
            .AsAServer("SubscriberServer")
            .OpenChannel("SubscriberChannel")
            .ForSubscribingTo("PublisherChannel")
            .RepeatMessages().Every(TimeSpan.FromSeconds(RepeatSeconds))
            .Initialise();

        It should_send_the_subscription_request_containing_the_strategy_with_the_expected_repeat_time = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetSubscriptionRequestSchema()
                    .RepeatStrategy.As<ConstantTimeRepeatStrategy>()
                        .RepeatEvery.ShouldEqual(TimeSpan.FromSeconds(RepeatSeconds));
    }
}