using System;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.messenger
{
    [Subject(SpecificationGroup.Description)]
    public class when_unregistering_an_unregistered_action_and_then_sending_a_message : WithSubject<TaskRepeater>
    {
        static object handledMessage;
        static object message;

        Establish context = () =>
        {
            Action<object> action = m => handledMessage = m;
            Messenger.Unregister(action);
            message = new object();
        };

        Because of = () => Messenger.Send(message);

        It should_not_handle_the_message = () => handledMessage.ShouldBeNull();
    }
}