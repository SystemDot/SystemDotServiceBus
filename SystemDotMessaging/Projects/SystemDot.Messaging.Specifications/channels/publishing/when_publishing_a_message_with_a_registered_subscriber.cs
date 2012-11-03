using System;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

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
                .SentMessages.ShouldContain(inputMessage);

        It should_process_the_message = () => processedMessage.ShouldBeTheSameAs(message);
        
        It should_reset_the_last_time_sent = () => processedMessage.HasHeader<LastSentHeader>().ShouldBeFalse();
    }
}