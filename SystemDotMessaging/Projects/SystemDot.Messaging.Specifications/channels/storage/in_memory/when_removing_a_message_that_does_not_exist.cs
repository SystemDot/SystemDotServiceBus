using System;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.storage.in_memory
{
    [Subject("Message storage")]
    public class when_removing_a_message_that_does_not_exist : WithSubject<PersistenceFactory>
    {
        static Exception exception;
        static IPersistence persistence;

        Establish context = () =>
            persistence = Subject.CreatePersistence(
                PersistenceUseType.SubscriberRequestSend,
                new EndpointAddress("Channel", "Server"));

        Because of = () => exception = Catch.Exception(() => persistence.Delete(Guid.NewGuid()));

        It should_not_let_the_message_pass_through = () => exception.ShouldBeNull();
    }
}
