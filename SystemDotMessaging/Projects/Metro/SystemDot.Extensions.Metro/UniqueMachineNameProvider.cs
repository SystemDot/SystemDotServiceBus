using System;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace SystemDot
{
    public static class UniqueMachineNameProvider
    {
        public static string GetUniqueName()
        {
#if (NON_UNIQUE_MACHINE_NAMES)
            return GetMachineName();
#else
            return string.Format("{0}-{1}", GetMachineName(), GetUniqueId());
#endif
        }

        static string GetUniqueId()
        {
            return new EasClientDeviceInformation().Id.ToString();
        }

        static string GetMachineName()
        {
            return Environment.MachineName;
        }
    }
}