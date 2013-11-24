using System;
using System.Linq;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes.Upcasting;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.upcasting_changes
{
    [Subject(SpecificationGroup.Description)]
    public class when_upcasting_message_cache_checkpoint_changes_to_version_one : WithMessageConfigurationSubject
    {
        const string ChangeRootId = "ChangeRootId";
        static InMemoryChangeStore changeStore;
        
        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore(new JsonSerialiser(), new ChangeUpcasterRunner());
        };

        Because of = () => changeStore.StoreChange(ChangeRootId, new MessageCheckpointChange { Version = 0 });

        It should_upcast_the_checkpoint_change_cached_on_to_the_current_date = () => 
            changeStore.GetChanges(ChangeRootId)
                .OfType<MessageCheckpointChange>()
                    .Single().CachedOn.ShouldEqual(new DateTime(2013, 11, 24));
    }
}