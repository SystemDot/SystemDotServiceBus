using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.messenger
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_with_a_handler_registered : WithSubject<TaskRepeater>
    {
        static object handledMessage;
        static object message;

        Establish context = () =>
        {
            Messenger.Register<object>(m => handledMessage = m);
            message = new object();
        };

        Because of = () => Messenger.Send(message);

        It should_handle_the_message = () => handledMessage.ShouldBeTheSameAs(message);
    }
}