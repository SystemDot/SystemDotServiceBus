using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Publishing.Builders;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Publishing
{
    public class PersistentSubscriberCollection : ChangeRoot, IEnumerable<Subscriber>
    {
        readonly EndpointAddress address;
        readonly ISubscriberSendChannelBuilder builder;
        readonly ConcurrentDictionary<EndpointAddress, Subscriber> subscribers;

        public PersistentSubscriberCollection(EndpointAddress address, IChangeStore changeStore, ISubscriberSendChannelBuilder builder) 
            : base(changeStore)
        {
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);
            Contract.Requires(changeStore != null);
            Contract.Requires(builder != null);

            this.address = address;
            this.builder = builder;
            this.subscribers = new ConcurrentDictionary<EndpointAddress, Subscriber>();
            
            Id = address.ToString();
            Initialise();
        }

        public void AddSubscriber(SubscriptionSchema schema)
        {
            AddChange(new SubscribeChange { Schema = schema });
        }

        public void ApplyChange(SubscribeChange change)
        {
            var subscriber = new Subscriber(this.builder);

            if(this.subscribers.TryAdd(change.Schema.SubscriberAddress, subscriber))
            {
                subscriber.BuildChannel(this.address, change.Schema);
            }
        }

        public IEnumerator<Subscriber> GetEnumerator()
        {
            return this.subscribers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected override void UrgeCheckPoint()
        {
        }
    }
}