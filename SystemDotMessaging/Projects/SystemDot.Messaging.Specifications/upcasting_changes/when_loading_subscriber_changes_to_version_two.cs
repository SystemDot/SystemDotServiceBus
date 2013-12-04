using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Publishing;
using SystemDot.Messaging.Repeating;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.upcasting_changes
{
    [Subject(SpecificationGroup.Description)]
    public class when_loading_subscriber_changes_to_version_two : WithMessageConfigurationSubject
    {
        const string ChangeRootId = "ChangeRootId";
        static InMemoryChangeStore changeStore;
        static IEnumerable<Change> loadedChanges;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();

            changeStore.StoreRawChange(
                ChangeRootId, 
                new SubscribeChange
                {
                    Version = 1, 
                    Schema = new SubscriptionSchema()
                });
        };

        Because of = () => loadedChanges = changeStore.GetChanges(ChangeRootId);

        It should_upcast_the_subscriber_change_to_include_the_default_repeat_strategy = () =>
            loadedChanges.OfType<SubscribeChange>()
                .Single().Schema.RepeatStrategy
                    .As<ConstantTimeRepeatStrategy>()
                    .RepeatEvery.ShouldEqual(TimeSpan.FromSeconds(10));
    }
}