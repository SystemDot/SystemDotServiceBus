using SystemDot.Messaging.Channels.Filtering;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.filtering
{
    [Subject("Message filtering")]
    public class when_filtering_a_message_by_namespace_that_is_not_in_the_specified_namespace : WithSubject<MessageFilter>
    {
        static TestMessage message;
        static TestMessage processed;

        Establish context = () =>
        {
            Subject = new MessageFilter(new NamespaceMessageFilterStrategy("OtherNamespace"));

            Subject.MessageProcessed += i => processed = i.As<TestMessage>();
            message = new TestMessage();
        };

        Because of = () => Subject.InputMessage(message);

        It should_not_pass_the_message_through = () => processed.ShouldBeNull();
    }
}