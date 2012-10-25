using System;
using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;
using SystemDot.Messaging.Channels.Repeating;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject("Message publishing")]
    public class when_publishing_a_message_with_a_registered_subscriber : WithSubject<Publisher>
    {
        static MessagePayload inputMessage;
        static SubscriptionSchema subscriber;
        static MessagePayload message;
        static MessagePayload processedMessage;

        Establish context = () =>
        {
            Configure(new EndpointAddress("Publisher", "Server"));
            Configure<ISerialiser>(new PlatformAgnosticSerialiser());
            Configure<IMessageSender>(new TestMessageSender());
            Configure<ISubscriberSendChannelBuilder>(new TestSubscriberSendChannelBuilder(The<IMessageSender>()));

            subscriber = new SubscriptionSchema { SubscriberAddress = new EndpointAddress("Subsrcriber", "Server") };
            Subject.Subscribe(subscriber);
            
            Subject.MessageProcessed += m => processedMessage = m;

            message = new MessagePayload();
            message.SetLastTimeSent(DateTime.Now);
            message.IncreaseAmountSent();
            inputMessage = message;
        };

        Because of = () => Subject.InputMessage(inputMessage);


        It should_pass_an_equivelent_message_to_the_subscriber = () => 
            The<IMessageSender>()
                .As<TestMessageSender>()
                .SentMessages.ShouldContain(m => 
                    m.GetFromAddress() == The<EndpointAddress>()
                    && m.GetToAddress() == subscriber.SubscriberAddress);

        It should_copy_the_message_to_the_subscriber = () => 
            The<IMessageSender>()
                .As<TestMessageSender>()
                .SentMessages.First()
                .ShouldNotBeTheSameAs(message);

        It should_remove_the_last_time_sent_and_amount_sent_from_the_copied_message = () =>
            The<IMessageSender>()
                .As<TestMessageSender>()
                .SentMessages.First().Headers.OfType<LastSentHeader>()
                .ShouldBeEmpty();

        It should_process_the_message = () => processedMessage.ShouldBeTheSameAs(message);

    }
}