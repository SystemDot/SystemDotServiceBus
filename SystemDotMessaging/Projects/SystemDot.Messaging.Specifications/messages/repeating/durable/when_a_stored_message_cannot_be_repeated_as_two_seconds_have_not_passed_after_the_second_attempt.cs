using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Messages.Processing.Repeating;
using SystemDot.Messaging.Messages.Storage;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.repeating.durable
{
    public class when_a_stored_message_cannot_be_repeated_as_two_seconds_have_not_passed_after_the_second_attempt
        : WithSubject<DurableMessageRepeater>
    {
        static MessagePayload processedMessage;
        static MessagePayload message;

        Establish context = () =>
        {
            var endpointAddress = new EndpointAddress("Channel", "Server");
            Configure<IMessageCache>(new MessageCache(new InMemoryPersistence(), endpointAddress));
            
            DateTime currentDate = DateTime.Now;
            Configure<ICurrentDateProvider>(new TestCurrentDateProvider(currentDate));

            Subject.MessageProcessed += m => processedMessage = m;

            message = new MessagePayload();
            message.SetFromAddress(endpointAddress);
            message.SetLastTimeSent(currentDate.Subtract(new TimeSpan(0, 0, 0, 0, 1999)));
            message.IncreaseAmountSent();
            message.IncreaseAmountSent();
            The<IMessageCache>().Cache(message);
        };

        Because of = () => Subject.Start();

        It should_output_the_message = () => processedMessage.ShouldBeNull();
    }
}