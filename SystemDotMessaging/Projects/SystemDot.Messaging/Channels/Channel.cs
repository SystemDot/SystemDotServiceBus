using System;

namespace SystemDot.Messaging.Channels
{
    public class Channel : IPipe
    {
        readonly IDistributor distributor;

        public event Action<object> MessagePublished;
        
        public Channel(IDistributor distributor)
        {
            this.distributor = distributor;
        }

        public void Start()
        {
            this.distributor.Start(OnMessageDistributed);
        }

        public void OnMessageDistributed(object message)
        {
            if(MessagePublished != null)
                MessagePublished(message);
        }

        public void Stop()
        {
            this.distributor.Stop();
        }

        public void Publish(object message)
        {
            this.distributor.Distribute(message);
        }
    }
}