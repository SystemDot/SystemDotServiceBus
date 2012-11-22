using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Threading;
using SystemDot.Messaging.Channels.Addressing;

namespace SystemDot.Messaging.Channels.Distribution
{
    public class ChannelDistributor<T> : IMessageInputter<T>
    {
        readonly ConcurrentDictionary<EndpointAddress, ChannelContainer> channels;
        readonly IChannelDistrbutionStrategy<T> distrbutionStrategy;

        public ChannelDistributor(IChannelDistrbutionStrategy<T> distrbutionStrategy)
        {
            Contract.Requires(distrbutionStrategy != null);

            this.distrbutionStrategy = distrbutionStrategy;
            this.channels = new ConcurrentDictionary<EndpointAddress, ChannelContainer>();
        }

        public void InputMessage(T toInput)
        {
            this.distrbutionStrategy.Distribute(this, toInput);
        }

        public IMessageInputter<T> GetChannel(EndpointAddress address)
        {
            return this.channels[address].GetChannel();
        }

        public void RegisterChannel(EndpointAddress address, Func<IMessageInputter<T>> toRegister)
        {
            var channelContainer = new ChannelContainer(toRegister);

            if (this.channels.TryAdd(address, channelContainer))
                channelContainer.BuildChannel();
        }

        class ChannelContainer
        {
            readonly object locker;
            readonly Func<IMessageInputter<T>> channelBuilder;

            IMessageInputter<T> channel;

            public ChannelContainer(Func<IMessageInputter<T>> channelBuilder)
            {
                this.channelBuilder = channelBuilder;
                this.locker = new object();
            }

            public void BuildChannel()
            {
                this.channel = this.channelBuilder.Invoke();

                lock (this.locker) 
                    Monitor.Pulse(this.locker);
            }

            public IMessageInputter<T> GetChannel()
            {
                if (this.channel == null)
                    lock (this.locker) 
                        Monitor.Wait(this.locker);
                
                return this.channel;
            }
        }
    }
}