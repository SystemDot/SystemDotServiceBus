using SystemDot.Messaging.Channels.Filtering;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.filtering
{
    [Subject("Message filtering")]
    public class when_filtering_a_message_by_name_pattern_that_does_not_match_the_specified_pattern : WithSubject<MessageFilter>
    {
        static TestMessage message;
        static TestMessage processed;

        Establish context = () =>
        {
            Subject = new MessageFilter(new NamePatternMessageFilterStrategy("NotTest"));

            Subject.MessageProcessed += i => processed = i.As<TestMessage>();
            message = new TestMessage();
        };

        Because of = () => Subject.InputMessage(message);

        It should_not_pass_the_message_through = () => processed.ShouldBeNull();
    }
}