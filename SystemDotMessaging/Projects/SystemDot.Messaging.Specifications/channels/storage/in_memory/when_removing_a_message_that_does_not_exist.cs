using System;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.storage.in_memory
{
    [Subject("Message handling")]
    public class when_removing_a_message_that_does_not_exist : WithSubject<InMemoryPersistence>
    {
        static Exception exception;

        Because of = () => exception = Catch.Exception(() => Subject.RemoveMessage(Guid.NewGuid()));

        It should_not_let_the_message_pass_through = () => exception.ShouldBeNull();
    }
}
