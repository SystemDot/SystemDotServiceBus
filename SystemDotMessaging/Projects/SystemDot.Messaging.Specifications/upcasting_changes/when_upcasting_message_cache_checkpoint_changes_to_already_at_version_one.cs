using System;
using System.Linq;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes.Upcasting;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.upcasting_changes
{
    [Subject(SpecificationGroup.Description)]
    public class when_upcasting_message_cache_checkpoint_changes_to_already_at_version_one : WithMessageConfigurationSubject
    {
        const string ChangeRootId = "ChangeRootId";

        static InMemoryChangeStore changeStore;
        static DateTime originalCachedOn;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore(new JsonSerialiser(), new ChangeUpcasterRunner());
            originalCachedOn = SystemTime.GetCurrentDate().AddDays(1);
        };

        Because of = () => changeStore.StoreChange(ChangeRootId, new MessageCheckpointChange { Version = 1, CachedOn = originalCachedOn });

        It should_not_upcast_the_checkpoint_change_cached_on_to_the_current_date = () =>
            changeStore.GetChanges(ChangeRootId)
                .OfType<MessageCheckpointChange>()
                .Single().CachedOn.ShouldEqual(originalCachedOn);
    }
}