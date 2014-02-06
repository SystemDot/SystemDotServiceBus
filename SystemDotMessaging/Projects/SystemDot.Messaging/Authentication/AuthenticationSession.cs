using System;
using System.Globalization;
using SystemDot.Core;

namespace SystemDot.Messaging.Authentication
{
    public class AuthenticationSession : Equatable<AuthenticationSession>
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public TimeSpan ExpiresAfter { get; set; }

        public DateTime ExpiresOn { get; set; }

        public AuthenticationSession()
        {
        }

        public AuthenticationSession(TimeSpan expiresAfter)
        {
            Id = Guid.NewGuid();
            CreatedOn = SystemTime.Current.GetCurrentDate();
            ExpiresAfter = expiresAfter;
            RecalculateExpiresOn();
        }

        public bool NeverExpires()
        {
            return ExpiresAfter == TimeSpan.MaxValue;
        }

        internal void RecalculateExpiresOn()
        {
            ExpiresOn = ExpiresAfter == TimeSpan.MaxValue
                ? DateTime.MaxValue
                : SystemTime.Current.GetCurrentDate().Add(ExpiresAfter);
        }

        public override bool Equals(AuthenticationSession other)
        {
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format(
                "Id: {0}, Created on: {1}, Expiry: {2}", 
                Id, 
                CreatedOn, 
                GetExpiresInToString());
        }

        string GetExpiresInToString()
        {
            return ExpiresAfter == TimeSpan.MaxValue 
                ? "Never" 
                : ExpiresAfter.TotalMinutes.ToString(CultureInfo.InvariantCulture) + " minutes";
        }
    }
}