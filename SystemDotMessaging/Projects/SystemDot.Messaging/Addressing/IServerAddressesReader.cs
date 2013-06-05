using System.Collections.Generic;

namespace SystemDot.Messaging.Addressing
{
    public interface IServerAddressesReader
    {
        Dictionary<string, string> LoadAddresses();
    }
}