using System;
using System.Collections.Generic;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Messages.Processing.Repeating;
using SystemDot.Specifications;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.repeating
{
    [Subject("Message repeating")]
    public class when_an_inputted_message_is_sent_twice_and_the_specified_time_period_has_passed_again : WithSubject<MessageRepeater>
    {
        static MessagePayload message;
        static List<MessagePayload> processedMessages;
        static TestTaskScheduler scheduler;
        static TimeSpan delay;

        Establish context = () =>
        {
            message = new MessagePayload();
            delay = new TimeSpan(0, 0, 1);
            
            scheduler = new TestTaskScheduler(2, new TestCurrentDateProvider(DateTime.Now));
            Subject = new MessageRepeater(delay, scheduler);
            processedMessages = new List<MessagePayload>();

            Subject.MessageProcessed += m => processedMessages.Add(m);
        };

        Because of = () => Subject.InputMessage(message);

        It should_output_the_message_thrice = () => processedMessages.Count.ShouldEqual(3);

        It should_output_the_message_for_the_second_time_after_the_specified_time_period_has_elapsed = () =>
            scheduler.LastDelay.ShouldEqual(new TimeSpan(0, 0, 2));

    }
}