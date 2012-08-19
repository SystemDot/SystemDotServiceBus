using SystemDot.Messaging.Messages.Processing.Filtering;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.processing.filtering
{
    [Subject("Message filtering")]
    public class when_filtering_a_message_by_name_pattern_that_matches_the_specified_pattern : WithSubject<MessageFilter>
    {
        static TestMessage message;
        static TestMessage processed;

        Establish context = () =>
        {
            Subject = new MessageFilter(new NamePatternMessageFilterStrategy("Test"));

            Subject.MessageProcessed += i => processed = i.As<TestMessage>();
            message = new TestMessage();
        };

        Because of = () => Subject.InputMessage(message);

        It should_pass_the_message_through = () => processed.ShouldBeTheSameAs(message);
    }
}