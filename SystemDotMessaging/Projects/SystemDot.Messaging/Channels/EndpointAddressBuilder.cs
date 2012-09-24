using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Channels
{
    public class EndpointAddressBuilder
    {
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
                    string.Concat(channelParts[0], "@", Environment.MachineName),
                    serverName);

            return new EndpointAddress(addressParts[0], serverName);
        }
    }
}