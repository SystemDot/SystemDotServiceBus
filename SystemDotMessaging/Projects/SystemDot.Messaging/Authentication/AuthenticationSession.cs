using System;
using System.Globalization;

namespace SystemDot.Messaging.Authentication
{
    public class AuthenticationSession : Equatable<AuthenticationSession>
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ExpiresOn { get; set; }

        public AuthenticationSession()
        {
        }

        public AuthenticationSession(DateTime expiresOn)
        {
            Id = Guid.NewGuid();
            CreatedOn = SystemTime.Current.GetCurrentDate();
            ExpiresOn = expiresOn;
        }

        public bool NeverExpires()
        {
            return ExpiresOn == DateTime.MaxValue;
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
            return String.Format("Id: {0}, Created on: {1}, Expires on: {2}", Id, CreatedOn, GetExpiresOnToString());
        }

        string GetExpiresOnToString()
        {
            return ExpiresOn == DateTime.MaxValue ? "Never" : ExpiresOn.ToString(CultureInfo.InvariantCulture);
        }
    }
}