using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Configuration.HttpMessaging
{
    public class MessageServerConfiguration : Configurer
    {
        readonly string messageServerName;

        public MessageServerConfiguration(string messageServerName)
        {
            Contract.Requires(!string.IsNullOrEmpty(messageServerName));
            
            this.messageServerName = messageServerName;
        }

        public ChannelConfiguration OpenChannel(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            return new ChannelConfiguration(
                BuildEndpointAddress(name, this.messageServerName), 
                this.messageServerName, 
                new List<Action>());
        }
    }
}