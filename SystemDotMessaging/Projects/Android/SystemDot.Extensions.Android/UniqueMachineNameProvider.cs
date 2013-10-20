using System;
using SystemDot.Configuration;
using Android.Provider;

namespace SystemDot
{
    public static class UniqueMachineNameProvider
    {
        public static string GetUniqueName()
        {
            return string.Format("{0}-{1}", GetMachineName(), GetUniqueId());
        }

        static string GetUniqueId()
        {
            return Settings.Secure.GetString(MainActivityLocator.Locate().ContentResolver, Settings.Secure.AndroidId);
        }

        static string GetMachineName()
        {
            return Android.OS.Build.Model;
        }
    }
}