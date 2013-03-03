using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.messenger
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_with_two_handlers_registered : WithSubject<TaskRepeater>
    {
        static object handledMessage1;
        static object handledMessage2;
        static object message;

        Establish context = () =>
        {
            Messenger.Register<object>(m => handledMessage1 = m);
            Messenger.Register<object>(m => handledMessage2 = m);

            message = new object();
        };

        Because of = () => Messenger.Send(message);

        It should_handle_the_message_with_the_first_handler = () => handledMessage1.ShouldBeTheSameAs(message);

        It should_handle_the_message_with_the_second_handler = () => handledMessage2.ShouldBeTheSameAs(message);
    }
}