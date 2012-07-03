using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.publishing
{
    [Subject("Message publishing")]
    public class when_starting_a_subscription_requestor : WithSubject<SubscriptionRequestor>
    {
        static EndpointAddress address;
        static MessagePayload request;

        Establish context = () =>
        {
            address = new EndpointAddress("PublisherAddress");
            Configure<EndpointAddress>(address);
            Subject.MessageProcessed += m => request = m;
        };

        Because of = () => Subject.Start();

        It should_output_a_subscription_request = () => request.IsSubscriptionRequest().ShouldBeTrue();

        It should_put_the_subscriber_address_on_request_schema = () =>
            request.GetSubscriptionRequestSchema().SubscriberAddress.ShouldEqual(address);
    }
}