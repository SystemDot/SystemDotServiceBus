using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Messages
{
    public class EndpointAddressBuilder
    {
        readonly IMachineIdentifier machineIdentifier;

        public EndpointAddressBuilder(IMachineIdentifier machineIdentifier)
        {
            Contract.Requires(machineIdentifier != null);

            this.machineIdentifier = machineIdentifier;
        }

        public EndpointAddress Build(string address, string messageServerName)
        {
            Contract.Requires(!string.IsNullOrEmpty(address));
            Contract.Requires(!string.IsNullOrEmpty(messageServerName));

            string serverName = messageServerName;
            
            string[] addressParts = address.Split('.');
            if (addressParts.Length == 2)
                serverName = addressParts[1];

            string[] channelParts = addressParts[0].Split('@');
            
            if(channelParts.Length == 1)
                return new EndpointAddress(
                    string.Concat(channelParts[0], "@", this.machineIdentifier.GetMachineName()),
                    serverName);

            return new EndpointAddress(addressParts[0], serverName);
        }
    }
}