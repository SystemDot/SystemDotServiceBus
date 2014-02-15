using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Diagnostics;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Storage.Changes;
using FluentAssertions;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.diagnostics
{
    [Subject(SpecificationGroup.Description)]
    public class when_describing_all_changes_in_the_change_store_with_muliple_changes_stored : WithMessageConfigurationSubject
    {
        static InMemoryChangeStore changeStore;
        static IEnumerable<ChangeDescription> changes;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();
            ConfigureAndRegister<ChangeStore>(changeStore);

            changeStore.StoreRawChange("ChangeRootId", new AddMessageChange());
            changeStore.StoreRawChange("OtherChangeRootId", new DeleteMessageChange());
        };

        Because of = () => changes = Debug.DescribeAllChangeStoreChanges();

        It should_return_the_changes = () => changes.Count().ShouldBeEquivalentTo(2);
    }
}