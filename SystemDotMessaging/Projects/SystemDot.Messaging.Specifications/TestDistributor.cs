using System.Collections.Generic;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Specifications
{
    public class TestDistributor : IDistributor 
    {
        public List<IMessageInputter<MessagePayload>> Subscribers { get; private set; }

        public TestDistributor()
        {
            Subscribers = new List<IMessageInputter<MessagePayload>>();
        }

        public void Subscribe(object key, IMessageInputter<MessagePayload> toSubscribe)
        {
            this.Subscribers.Add(toSubscribe);
        }

        public void InputMessage(MessagePayload toInput)
        {
            throw new System.NotImplementedException();
        }

        
    }
}