using System.Collections.Generic;
using SystemDot.Messaging.Channels.Messages.Distribution;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Specifications
{
    public class TestDistributor : IDistributor 
    {
        public List<IDistributionSubscriber> Subscribers { get; private set; }

        public TestDistributor()
        {
            Subscribers = new List<IDistributionSubscriber>();
        }

        public void Subscribe(IDistributionSubscriber toSubscribe)
        {
            this.Subscribers.Add(toSubscribe);
        }

        public void InputMessage(MessagePayload toInput)
        {
            throw new System.NotImplementedException();
        }
    }
}