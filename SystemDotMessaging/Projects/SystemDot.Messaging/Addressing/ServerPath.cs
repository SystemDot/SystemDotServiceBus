namespace SystemDot.Messaging.Addressing
{
    public class ServerPath
    {
        public MessageServer LocatedAt { get; set; }
        public MessageServer RoutedVia { get; set; }

        public ServerPath()
        {
        }

        public ServerPath(MessageServer serverName, MessageServer remoteProxy)
        {
            this.LocatedAt = serverName;
            this.RoutedVia = remoteProxy;
        }

        public static bool operator ==(ServerPath left, ServerPath right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ServerPath left, ServerPath right)
        {
            return !left.Equals(right);
        }

        protected bool Equals(ServerPath other)
        {
            return string.Equals(this.LocatedAt, other.LocatedAt) && string.Equals(this.RoutedVia, other.RoutedVia);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ServerPath) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.LocatedAt != null ? this.LocatedAt.GetHashCode() : 0)*397) ^ (this.RoutedVia != null ? this.RoutedVia.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Concat(LocatedAt, ".", RoutedVia);
        }
    }
}