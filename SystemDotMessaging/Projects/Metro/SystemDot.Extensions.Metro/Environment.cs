using System.Linq;
using Windows.Networking.Connectivity;

namespace SystemDot
{
    public class Environment
    {
        public static string MachineName
        {
            get { return NetworkInformation.GetHostNames().First().DisplayName.Split('@')[0]; }
        }
    }
}