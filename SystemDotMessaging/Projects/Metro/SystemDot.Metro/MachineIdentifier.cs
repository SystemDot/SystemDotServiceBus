using System.Linq;
using Windows.Networking.Connectivity;

namespace SystemDot
{
    public class MachineIdentifier : IMachineIdentifier
    {
        public string GetMachineName()
        {
            return NetworkInformation.GetHostNames().First().DisplayName;
        }
    }
}