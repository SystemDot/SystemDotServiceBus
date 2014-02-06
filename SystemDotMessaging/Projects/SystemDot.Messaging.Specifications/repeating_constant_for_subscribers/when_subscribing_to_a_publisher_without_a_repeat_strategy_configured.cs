using SystemDot.Core;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing;
using SystemDot.Messaging.Repeating;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating_constant_for_subscribers
{
    [Subject(SpecificationGroup.Description)]
    public class when_subscribing_to_a_publisher_without_a_repeat_strategy_configured : WithHttpConfigurationSubject
    {
        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport()
            .AsAServer("SubscriberServer")
            .OpenChannel("SubscriberChannel")
            .ForSubscribingTo("PublisherChannel")
            .Initialise();

        It should_send_the_subscription_request_containing_the_escalating_repeat_strategy_with_the_default_start = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetSubscriptionRequestSchema().RepeatStrategy.As<EscalatingTimeRepeatStrategy>()
                    .ToStartAt.ShouldEqual(4);

        It should_send_the_subscription_request_containing_the_escalating_repeat_strategy_with_the_default_multiplier = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetSubscriptionRequestSchema().RepeatStrategy.As<EscalatingTimeRepeatStrategy>()
                    .Multiplier.ShouldEqual(2);

        It should_send_the_subscription_request_containing_the_escalating_repeat_strategy_with_the_default_peak = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetSubscriptionRequestSchema().RepeatStrategy.As<EscalatingTimeRepeatStrategy>()
                    .Peak.ShouldEqual(16);
    }
}