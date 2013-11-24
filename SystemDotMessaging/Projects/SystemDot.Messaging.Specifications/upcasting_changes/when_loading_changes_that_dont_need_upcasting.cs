using System;
using System.Linq;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes.Upcasting;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.upcasting_changes
{
    [Subject(SpecificationGroup.Description)]
    public class when_loading_changes_that_dont_need_upcasting : WithMessageConfigurationSubject
    {
        const string ChangeRootId = "ChangeRootId";

        static InMemoryChangeStore changeStore;
        static AddMessageChange change;

        Establish context = () =>
        {
            change = new AddMessageChange { Version = 0 };
            changeStore = new InMemoryChangeStore(new JsonSerialiser(), new ChangeUpcasterRunner());
        };

        Because of = () => changeStore.StoreChange(ChangeRootId, change);

        It should_pass_through_the_change = () => changeStore.GetChanges(ChangeRootId).Count().ShouldEqual(1);
    }
}