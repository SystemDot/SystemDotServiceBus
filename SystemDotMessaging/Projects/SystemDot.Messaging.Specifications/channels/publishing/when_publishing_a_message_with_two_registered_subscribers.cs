using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject("Message publishing")]
    public class when_publishing_a_message_with_two_registered_subscribers : WithSubject<Publisher>
    {
        static MessagePayload inputMessage;
        static SubscriptionSchema subscriber1;
        static SubscriptionSchema subscriber2;
        
        Establish context = () =>
        {
            Configure(new EndpointAddress("Publisher", "Server"));
            Configure<ISerialiser>(new PlatformAgnosticSerialiser());
            Configure<IMessageSender>(new TestMessageSender());
            Configure<ISubscriberSendChannelBuilder>(new TestSubscriberSendChannelBuilder(The<IMessageSender>()));
    
            subscriber1 = new SubscriptionSchema { SubscriberAddress = new EndpointAddress("Subsrciber1", "Server")};
            Subject.Subscribe(subscriber1);

            subscriber2 = new SubscriptionSchema { SubscriberAddress = new EndpointAddress("Subscriber2", "Server") };
            Subject.Subscribe(subscriber2);

            inputMessage = new MessagePayload();
            
            Subject.MessageProcessed += _ => { };
        };

        Because of = () => Subject.InputMessage(inputMessage);

        It should_pass_the_message_to_both_subscribers = () =>
            The<IMessageSender>()
                .As<TestMessageSender>()
                .SentMessages
                .Count
                .ShouldEqual(2);
    }
}