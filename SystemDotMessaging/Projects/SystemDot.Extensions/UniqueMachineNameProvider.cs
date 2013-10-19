using System;

namespace SystemDot
{
    public static class UniqueMachineNameProvider
    {
        public static string GetUniqueName()
        {
            return Environment.MachineName;
        }
    }
}