using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Storage.Changes;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.upcasting_changes
{
    [Subject(SpecificationGroup.Description)]
    public class when_loading_message_cache_checkpoint_changes_to_already_at_latest_version 
        : WithMessageConfigurationSubject
    {
        const string ChangeRootId = "ChangeRootId";

        static InMemoryChangeStore changeStore;
        static DateTime originalCachedOn;
        static IEnumerable<Change> loadedChanges;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();
            originalCachedOn = SystemTime.GetCurrentDate().AddDays(1);
            changeStore.StoreRawChange(ChangeRootId, new MessageCheckpointChange { Version = 1, CachedOn = originalCachedOn });
        };

        Because of = () => loadedChanges = changeStore.GetChanges(ChangeRootId);

        It should_not_upcast_the_checkpoint_change_cached_on_to_the_current_date = () =>
            loadedChanges.OfType<MessageCheckpointChange>().Single().CachedOn.ShouldBeEquivalentTo(originalCachedOn);
    }
}