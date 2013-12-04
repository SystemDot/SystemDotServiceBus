using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Diagnostics;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.diagnostics
{
    [Subject(SpecificationGroup.Description)]
    public class when_describing_all_changes_in_the_change_store_with_one_change_stored : WithMessageConfigurationSubject
    {
        const string ChangeRootId = "ChangeRootId";

        static InMemoryChangeStore changeStore;
        static AddMessageChange change;
        static IEnumerable<ChangeDescription> changes;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();
            ConfigureAndRegister<ChangeStore>(changeStore);

            change = new AddMessageChange();
            changeStore.StoreRawChange(ChangeRootId, change);
        };

        Because of = () => changes = Debug.DescribeAllChangeStoreChanges();

        It should_return_the_change_root_id_for_the_change = () => changes.Single().RootId.ShouldEqual(ChangeRootId);

        It should_return_the_sequence_of_the_change = () => changes.Single().Sequence.ShouldEqual(1);

        It should_return_the_change_as_text = () => changes.Single().Change.ShouldEqual(Resolve<ISerialiser>().SerialiseToString(change));
    }
}