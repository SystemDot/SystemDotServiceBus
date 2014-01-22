using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Caching.Changes;
using SystemDot.Storage.Changes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.upcasting_changes
{
    [Subject(SpecificationGroup.Description)]
    public class when_loading_authentication_session_changes_to_version_two_from_version_zero : WithMessageConfigurationSubject
    {
        const string ChangeRootId = "ChangeRootId";
        static InMemoryChangeStore changeStore;
        static IEnumerable<Change> loadedChanges;
        static AuthenticationSessionCachedChange change;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();

            change = new AuthenticationSessionCachedChange
            {
                Version = 0,
                Server = new MessageServer(),
                Session = new AuthenticationSession
                {
                    Id = Guid.NewGuid(),
                    CreatedOn = SystemTime.GetCurrentDate(),
                    ExpiresOn = SystemTime.GetCurrentDate().AddDays(1).AddSeconds(1)
                }
            };

            changeStore.StoreRawChange(ChangeRootId, change);
        };

        Because of = () => loadedChanges = changeStore.GetChanges(ChangeRootId);

        It should_upcast_the_authentication_session_change_to_expire_on_the_next_day = () =>
            loadedChanges
                .OfType<AuthenticationSessionCachedChange>()
                .Single().Session
                .ExpiresAfter.ShouldEqual(change.Session.ExpiresOn.Subtract(change.Session.CreatedOn));
    }
}