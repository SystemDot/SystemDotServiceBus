using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Publishing;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject("Message publishing")]
    public class when_starting_a_subscription_requestor_with_persistence_indicated_on_the_schema : WithSubject<SubscriptionRequestor>
    {
        static EndpointAddress address;
        static MessagePayload request;

        Establish context = () =>
        {
            address = new EndpointAddress("PublisherAddress", "TestServer");
            Subject = new SubscriptionRequestor(address, true);
            Subject.MessageProcessed += m => request = m;
        };

        Because of = () => Subject.Start();
        
        It should_set_the_request_schema_to_indicate_persistence = () =>
            request.GetSubscriptionRequestSchema().IsPersistent.ShouldBeTrue();
    }
}