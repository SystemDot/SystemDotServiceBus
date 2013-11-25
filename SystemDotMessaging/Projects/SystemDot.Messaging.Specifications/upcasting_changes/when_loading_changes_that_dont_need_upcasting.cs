using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Upcasting;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.upcasting_changes
{
    [Subject(SpecificationGroup.Description)]
    public class when_loading_changes_that_dont_need_upcasting : WithMessageConfigurationSubject
    {
        const string ChangeRootId = "ChangeRootId";

        static InMemoryChangeStore changeStore;
        static IEnumerable<Change> loadedChanges;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();
            changeStore.StoreRawChange(ChangeRootId, new AddMessageChange { Version = 0 });
        };

        Because of = () => loadedChanges = changeStore.GetChanges(ChangeRootId);

        It should_pass_through_the_change = () => loadedChanges.Count().ShouldEqual(1);
    }
}