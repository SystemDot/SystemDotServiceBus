using System.Linq;
using Windows.Networking;
using Windows.Networking.Connectivity;

namespace SystemDot
{
    public class Environment
    {
        public static string MachineName
        {
            get { return NetworkInformation.GetHostNames().First(name => name.Type == HostNameType.DomainName).DisplayName.Split('.').First(); }
        }
    }
}