using System;
using System.Reflection;

namespace SystemDot
{
    public static class TypeExtensions
    {
        public static bool IsInstanceOfType(this Type type, object o)
        {
            return false;
        }

        public static MethodInfo GetMethod(this Type type, string name, Type[] types)
        {
            return null;
        }
    }
}
