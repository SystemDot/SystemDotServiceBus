using System;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.publishing
{
    [Subject("Message publishing")]
    public class when_handling_a_non_subscription_request_message_with_the_subscription_request_handler
        : WithMessageInputterSubject<SubscriptionRequestHandler>
    {
        static Exception exception;
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            Configure<PublisherRegistry>(new PublisherRegistry());
            
            Configure<ISubscriptionChannelBuilder>(
                new TestSubscriptionChannelBuilder(
                    new SubscriptionSchema(new EndpointAddress("TestAddress", "TestServer")), 
                    new Pipe<MessagePayload>()));

            messagePayload = new MessagePayload();
        };

        Because of = () => exception = Catch.Exception(() => Subject.InputMessage(messagePayload));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}