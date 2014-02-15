using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Storage.Changes;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.upcasting_changes
{
    [Subject(SpecificationGroup.Description)]
    public class when_loading_message_cache_checkpoint_changes_to_version_one : WithMessageConfigurationSubject
    {
        const string ChangeRootId = "ChangeRootId";
        static InMemoryChangeStore changeStore;
        static IEnumerable<Change> loadedChanges;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();
            changeStore.StoreRawChange(ChangeRootId, new MessageCheckpointChange { Version = 0 });
        };

        Because of = () => loadedChanges = changeStore.GetChanges(ChangeRootId);

        It should_upcast_the_checkpoint_change_cached_on_to_the_current_date = () => 
            loadedChanges.OfType<MessageCheckpointChange>()
                .Single().CachedOn.ShouldBeEquivalentTo(new DateTime(2013, 11, 24));
    }
}