using System.Collections.Generic;

namespace SystemDot.Messaging.Configuration
{
    public class ConsumerConfiguration
    {
        readonly ChannelConfiguration channelConfig;

        public ConsumerConfiguration(ChannelConfiguration channelConfig)
        {
            this.channelConfig = channelConfig;
        }

        internal IConsume Consumer { get; private set; }

        public ChannelConfiguration With<T>(IConsume<T> toRegister)
        {
            Consumer = toRegister;
            return this.channelConfig;
        }
    }
}