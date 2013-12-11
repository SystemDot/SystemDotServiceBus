using System;
using System.Collections.Concurrent;
using System.Threading;
using SystemDot.Messaging.Addressing;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Distribution
{
    abstract class ChannelDistributor<T> : ChangeRoot, IMessageInputter<T>
    {
        readonly ConcurrentDictionary<EndpointAddress, ChannelContainer> channels;

        protected ChannelDistributor(ChangeStore changeStore, ICheckpointStrategy checkPointStrategy)
            : base(changeStore, checkPointStrategy)
        {
            channels = new ConcurrentDictionary<EndpointAddress, ChannelContainer>();
        }

        public void InputMessage(T toInput)
        {
            Distribute(toInput);
        }

        protected abstract void Distribute(T toDistibute);

        protected IMessageInputter<T> GetChannel(EndpointAddress address)
        {
            return channels[address].GetChannel();
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

            if (channels.TryAdd(address, channelContainer))
                channelContainer.BuildChannel();
        }

        protected override void UrgeCheckPoint()
        {
        }

        class ChannelContainer
        {
            readonly Func<IMessageInputter<T>> channelBuilder;
            readonly object locker;

            IMessageInputter<T> channel;

            public ChannelContainer(Func<IMessageInputter<T>> channelBuilder)
            {
                this.channelBuilder = channelBuilder;
                locker = new object();
            }

            public void BuildChannel()
            {
                channel = channelBuilder.Invoke();

                lock (locker)
                    Monitor.Pulse(locker);
            }

            public IMessageInputter<T> GetChannel()
            {
                if (channel == null)
                    lock (locker)
                        Monitor.Wait(locker);

                return channel;
            }
        }
    }
}