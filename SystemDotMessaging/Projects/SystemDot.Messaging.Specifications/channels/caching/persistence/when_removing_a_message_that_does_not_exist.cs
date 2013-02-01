using System;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.caching.persistence
{
    [Subject(SpecificationGroup.Description)]
    public class when_removing_a_message_that_does_not_exist : WithSubject<MessageCacheFactory>
    {
        static Exception exception;
        static MessageCache messageCache;

        Establish context = () =>
        {
            Configure<IChangeStore>(new InMemoryChangeStore(new PlatformAgnosticSerialiser()));
            
            messageCache = Subject.CreateCache(
                PersistenceUseType.SubscriberRequestSend, 
                new EndpointAddress("GetChannel", "Server"));
        };

        Because of = () => exception = Catch.Exception(() => messageCache.Delete(Guid.NewGuid()));

        It should_not_let_the_message_pass_through = () => exception.ShouldBeNull();
    }
}
