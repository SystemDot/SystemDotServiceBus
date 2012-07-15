using System;

namespace SystemDot
{
    public class MachineIdentifier : IMachineIdentifier
    {
        public string GetMachineName()
        {
            return Environment.MachineName;
        }
    }
}