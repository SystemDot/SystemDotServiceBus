using System;
using SystemDot.Messaging.Authentication.Caching.Changes;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Upcasting;

namespace SystemDot.Messaging.ChangeUpcasting.ToVersionTwo
{
    public class AuthenticationSessionCachedChangeUpcaster : IChangeUpcaster
    {
        public Type ChangeType { get { return typeof(AuthenticationSessionCachedChange); } }

        public int Version { get { return 1; } }

        public Change Upcast(Change toUpcast)
        {
            AuthenticationSessionCachedChange change = GetChange(toUpcast);

            change.Session.ExpiresAfter = CalculateExpiresAfter(change);

            return toUpcast;
        }

        static TimeSpan CalculateExpiresAfter(AuthenticationSessionCachedChange change)
        {
            return change.Session.ExpiresOn != DateTime.MaxValue 
                ? change.Session.ExpiresOn.Subtract(change.Session.CreatedOn)
                : TimeSpan.MaxValue;
        }

        static AuthenticationSessionCachedChange GetChange(Change toUpcast)
        {
            return toUpcast.As<AuthenticationSessionCachedChange>();
        }
    }
}