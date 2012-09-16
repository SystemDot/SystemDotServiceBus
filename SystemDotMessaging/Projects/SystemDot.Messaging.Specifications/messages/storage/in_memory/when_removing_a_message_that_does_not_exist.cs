using System;
using SystemDot.Messaging.Messages.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.storage.in_memory
{
    [Subject("Message handling")]
    public class when_removing_a_message_that_does_not_exist : WithSubject<InMemoryMessageStore>
    {
        static Exception exception;

        Because of = () => exception = Catch.Exception(() => Subject.Remove(Guid.NewGuid()));

        It should_not_let_the_message_pass_through = () => exception.ShouldBeNull();
    }
}
