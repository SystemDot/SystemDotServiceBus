using System;

namespace SystemDot
{
    public abstract class Equatable<T> : IEquatable<T>
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((T)obj);
        }

        public abstract bool Equals(T other);

        public abstract override int GetHashCode();

        public static bool operator ==(Equatable<T> left, Equatable<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Equatable<T> left, Equatable<T> right)
        {
            return !Equals(left, right);
        }
    }
}