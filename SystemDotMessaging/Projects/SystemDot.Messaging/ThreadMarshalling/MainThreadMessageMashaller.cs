using System;
using System.Diagnostics.Contracts;
using SystemDot.ThreadMashalling;

namespace SystemDot.Messaging.ThreadMarshalling
{
    class MainThreadMessageMashaller : IMessageProcessor<object, object>
    {
        readonly IMainThreadMarshaller marshaller;

        public MainThreadMessageMashaller(IMainThreadMarshaller marshaller)
        {
            Contract.Requires(marshaller != null);
            this.marshaller = marshaller;
        }

        public void InputMessage(object toInput)
        {
            marshaller.RunOnMainThread(() => OnMessageProcessed(toInput));
        }

        void OnMessageProcessed(object toProcess)
        {
            if (MessageProcessed != null) MessageProcessed(toProcess);
        }

        public event Action<object> MessageProcessed;
    }
}