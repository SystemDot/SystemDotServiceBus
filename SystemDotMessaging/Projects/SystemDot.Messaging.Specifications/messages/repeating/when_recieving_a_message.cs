using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Processing;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.repeating
{
    public class when_recieving_a_message : WithSubject<MessageRepeater>
    {
        static MessagePayload message;
        static MessagePayload processedMessage;
        static TestTaskScheduler scheduler;

        Establish context = () =>
        {
            message = new MessagePayload();
            
            var delay = new TimeSpan(0, 0, 1);
            scheduler = new TestTaskScheduler(0);
            Subject = new MessageRepeater(delay, scheduler);

            Subject.MessageProcessed += m => processedMessage = m;

        };

        Because of = () => Subject.InputMessage(message);

        It should_output_the_message = () => message.ShouldEqual(processedMessage);
        
        It should_output_the_message_immediately = () => scheduler.LastDelay.Ticks.ShouldEqual(0);
    }
}