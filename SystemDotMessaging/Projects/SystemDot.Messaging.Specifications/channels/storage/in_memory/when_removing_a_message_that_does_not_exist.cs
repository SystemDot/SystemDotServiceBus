using System;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.storage.in_memory
{
    [Subject("Message storage")]
    public class when_removing_a_message_that_does_not_exist : WithSubject<InMemoryPersistenceFactory>
    {
        static Exception exception;
        static IPersistence persistence;

        Establish context = () =>
            persistence = Subject.CreatePersistence(
                PersistenceUseType.Other,
                new EndpointAddress("Channel", "Server"));

        Because of = () => exception = Catch.Exception(() => persistence.Delete(Guid.NewGuid()));

        It should_not_let_the_message_pass_through = () => exception.ShouldBeNull();
    }
}
