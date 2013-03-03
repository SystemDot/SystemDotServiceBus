using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.messenger
{
    [Subject(SpecificationGroup.Description)]
    public class when_resetting_the_messenger_after_registering_a_handler_and_then_sending_a_message : WithSubject<TaskRepeater>
    {
        static object handledMessage;
        static object message;

        Establish context = () =>
        {
            Messenger.Register<object>(m => handledMessage = m);
            Messenger.Reset();
            message = new object();
        };

        Because of = () => Messenger.Send(message);

        It should_not_handle_the_message = () => handledMessage.ShouldBeNull();
    }
}