using System;
using System.Collections.Concurrent;
using System.Threading;
using SystemDot.Messaging.Addressing;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Upcasting;

namespace SystemDot.Messaging.Distribution
{
    abstract class ChannelDistributor<T> : ChangeRoot, IMessageInputter<T>
    {
        readonly ConcurrentDictionary<EndpointAddress, ChannelContainer> channels;

        protected ChannelDistributor(ChangeStore changeStore)
            : base(changeStore)
        {
            this.channels = new ConcurrentDictionary<EndpointAddress, ChannelContainer>();
        }

        public void InputMessage(T toInput)
        {
            Distribute(toInput);
        }

        protected abstract void Distribute(T toDistibute);

        public IMessageInputter<T> GetChannel(EndpointAddress address)
        {
            return this.channels[address].GetChannel();
        }

        public void RegisterChannel(EndpointAddress address)
        {
            if (ChannelExists(address)) return;
            AddRegisterChannelChange(address);
        }

        bool ChannelExists(EndpointAddress address)
        {
            return channels.ContainsKey(address);
        }

        protected abstract void AddRegisterChannelChange(EndpointAddress address);

        protected void RegisterChannel(EndpointAddress address, Func<IMessageInputter<T>> toRegister)
        {
            var channelContainer = new ChannelContainer(toRegister);

            if (this.channels.TryAdd(address, channelContainer))
                channelContainer.BuildChannel();
        }

        protected override void UrgeCheckPoint()
        {
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