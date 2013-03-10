using System;
using System.Reflection;

namespace SystemDot
{
    public static class AssemblyExtensions
    {
        public static string GetLocation(this Assembly assembly)
        {
            return "C:\\";
        }
    }
}