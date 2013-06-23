using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Addressing
{
    public class EndpointAddress
    {
        public static EndpointAddress Empty { get { return new EndpointAddress(); } }

        public string Channel { get; set; }

        public ServerRoute Route { get; set; }

        public EndpointAddress() {}

        public EndpointAddress(string channel, ServerRoute serverRoute) 
        {
            Contract.Requires(!string.IsNullOrEmpty(channel));
            Contract.Requires(serverRoute != null);

            this.Channel = channel;
            this.Route = serverRoute;
        }

        protected bool Equals(EndpointAddress other)
        {
            return string.Equals(Channel, other.Channel) 
                && string.Equals(Route, other.Route);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EndpointAddress) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Channel.GetHashCode()*397) ^ this.Route.GetHashCode();
            }
        }

        public static bool operator ==(EndpointAddress left, EndpointAddress right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EndpointAddress left, EndpointAddress right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return String.Concat(Channel, "@", this.Route);
        }
    }
}