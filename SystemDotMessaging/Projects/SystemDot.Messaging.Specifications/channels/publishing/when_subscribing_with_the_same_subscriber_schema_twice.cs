using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject("Message distribution")]
    public class when_subscribing_with_the_same_subscriber_schema_twice : WithSubject<Publisher>
    {
        static EndpointAddress address;
        static SubscriptionSchema subscriber1;
        static SubscriptionSchema subscriber2;
        static MessagePayload message;
        
        Establish context = () =>
        {
            Configure(new EndpointAddress("Publisher", "Server"));
            Configure(new MessagePayloadCopier(new PlatformAgnosticSerialiser()));
            Configure<ISerialiser>(new PlatformAgnosticSerialiser());
            Configure<IMessageSender>(new TestMessageSender());
            Configure<ISubscriberSendChannelBuilder>(new TestSubscriberSendChannelBuilder(The<IMessageSender>()));

            message = new MessagePayload();

            address = new EndpointAddress("TestAddress", "TestServer");
            subscriber1 = new SubscriptionSchema { SubscriberAddress = address };
            subscriber2 = new SubscriptionSchema { SubscriberAddress = address };
        
            Subject.Subscribe(subscriber1);
            Subject.Subscribe(subscriber2);

            Subject.MessageProcessed += _ => { };
        };

        Because of = () => Subject.InputMessage(message);

        It should_not_distribute_the_message_to_the_second_subscriber = () => 
            The<IMessageSender>()
                .As<TestMessageSender>()
                .SentMessages
                .Count.ShouldEqual(1);
    }
}