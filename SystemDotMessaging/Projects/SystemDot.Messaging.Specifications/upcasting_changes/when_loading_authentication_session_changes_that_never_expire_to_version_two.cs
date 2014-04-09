using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Caching.Changes;
using SystemDot.Storage.Changes;
using Machine.Specifications;
using FluentAssertions;

namespace SystemDot.Messaging.Specifications.upcasting_changes
{
    [Subject(SpecificationGroup.Description)]
    public class when_loading_authentication_session_changes_that_never_expire_to_version_two : WithMessageConfigurationSubject
    {
        const string ChangeRootId = "ChangeRootId";
        static InMemoryChangeStore changeStore;
        static IEnumerable<Change> loadedChanges;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();

            changeStore.StoreRawChange(
                ChangeRootId, 
                new AuthenticationSessionCachedChange
                {
                    Version = 1,
                    Server = new MessageServer(),
                    Session = new AuthenticationSession
                    {
                        Id = Guid.NewGuid(),
                        CreatedOn = SystemTime.GetCurrentDate(),
                        ExpiresOn = DateTime.MaxValue
                    }
                });
        };

        Because of = () => loadedChanges = changeStore.GetChanges(ChangeRootId);

        It should_upcast_the_authentication_session_change_expires_after_to_no_expiry = () =>
            loadedChanges
                .OfType<AuthenticationSessionCachedChange>()
                .Single().Session
                .ExpiresAfter.ShouldBeEquivalentTo(TimeSpan.MaxValue);
    }
}