using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Processing.Repeating;
using SystemDot.Messaging.Messages.Storage;
using SystemDot.Specifications;
using Machine.Fakes;
using Machine.Specifications;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Specifications.messages.repeating.durable
{
    public class when_a_stored_message_can_be_repeated_as_one_second_has_passed_after_the_first_attempt 
        : WithSubject<DurableMessageRepeater>
    {
        static MessagePayload processedMessage;
        static MessagePayload message;

        Establish context = () =>
        {
            DateTime currentDate = DateTime.Now;
            
            var endpointAddress = new EndpointAddress("Channel", "Server");
            Configure<IMessageCache>(new MessageCache(new InMemoryPersistence(), endpointAddress));
            Configure<ICurrentDateProvider>(new TestCurrentDateProvider(currentDate));
            
            Subject.MessageProcessed += m => processedMessage = m;

            message = new MessagePayload();
            message.SetFromAddress(endpointAddress);
            message.SetLastTimeSent(currentDate.Subtract(new TimeSpan(0, 0, 0, 1)));
            message.IncreaseAmountSent();
            The<IMessageCache>().Cache(message);
        };

        Because of = () => Subject.Start();

        It should_output_the_message = () => processedMessage.ShouldBeTheSameAs(message);

        It should_increase_the_amount_of_times_the_message_was_sent = () => processedMessage.GetAmountSent().ShouldEqual(2);
    }
}