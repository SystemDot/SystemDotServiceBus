using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject("Message publishing")]
    public class when_starting_a_subscription_requestor : WithSubject<SubscriptionRequestor>
    {
        static EndpointAddress address;
        static MessagePayload request;

        Establish context = () =>
        {
            address = new EndpointAddress("PublisherAddress", "TestServer");
            Subject = new SubscriptionRequestor(address, false);
            Subject.MessageProcessed += m => request = m;
        };

        Because of = () => Subject.Start();

        It should_output_a_subscription_request = () => request.IsSubscriptionRequest().ShouldBeTrue();

        It should_put_the_subscriber_address_on_request_schema = () =>
            request.GetSubscriptionRequestSchema().SubscriberAddress.ShouldEqual(address);

        It should_set_the_request_schema_to_indicate_no_persistence = () =>
           request.GetSubscriptionRequestSchema().IsDurable.ShouldBeFalse();
    }
}