using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Authentication
{
    public class AuthenticationSession : Equatable<AuthenticationSession>
    {
        public static AuthenticationSession FromPlan(DateTime currentDate, ExpiryPlan from)
        {
            Contract.Requires(@from != null);

            return new AuthenticationSession(GetExpiresOnFromPlan(currentDate, @from), GetGracePeriodEndOnFromPlan(currentDate, @from));
        }

        static DateTime GetGracePeriodEndOnFromPlan(DateTime currentDate, ExpiryPlan from)
        {
            return GetExpiresOnFromPlan(currentDate, @from).Add(@from.GracePeriod);
        }

        static DateTime GetExpiresOnFromPlan(DateTime currentDate, ExpiryPlan from)
        {
            return currentDate.Add(@from.AfterTime);
        }

        public Guid Id { get; set; }

        public DateTime ExpiresOn { get; set; }

        public DateTime GracePeriodEndOn { get; set; }

        public AuthenticationSession()
            : this(DateTime.MinValue, DateTime.MinValue)
        {
        }

        public AuthenticationSession(DateTime expiresOn, DateTime gracePeriodEndOn)
        {
            Id = Guid.NewGuid();
            ExpiresOn = expiresOn;
            GracePeriodEndOn = gracePeriodEndOn;
        }

        public override bool Equals(AuthenticationSession other)
        {
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}