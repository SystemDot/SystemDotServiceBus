using System;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Specifications;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.repeating
{
    public class when_a_stored_message_can_be_repeated_as_one_second_has_passed_after_the_first_attempt 
        : WithSubject<MessageRepeater>
    {
        static MessagePayload processedMessage;
        static MessagePayload message;

        Establish context = () =>
        {
            DateTime currentDate = DateTime.Now;
            
            var endpointAddress = new EndpointAddress("GetChannel", "Server");
            With<PersistenceBehaviour>();
            
            Configure<ICurrentDateProvider>(new TestCurrentDateProvider(currentDate));
            Configure<IRepeatStrategy>(new EscalatingTimeRepeatStrategy());
            
            Subject.MessageProcessed += m => processedMessage = m;

            message = new MessagePayload();
            message.SetSequence(1);
            message.SetFromAddress(endpointAddress);
            message.SetLastTimeSent(currentDate.Subtract(new TimeSpan(0, 0, 0, 1)));
            message.IncreaseAmountSent();
            The<MessageCache>().AddMessage(message);
        };

        Because of = () => Subject.Start();

        It should_output_the_message = () => processedMessage.ShouldEqual(message);

        It should_increase_the_amount_of_times_the_message_was_sent = () => processedMessage.GetAmountSent().ShouldEqual(2);
    }
}