using System;
using Windows.UI.Core;

namespace SystemDot.Messaging.TestSubscriber.ViewModels
{
    public class MessageMarshallingHook : IMessageProcessor<object, object>
    {
        readonly CoreDispatcher dispatcher;

        public MessageMarshallingHook(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public void InputMessage(object toInput)
        {
            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MessageProcessed(toInput));
        }

        public event Action<object> MessageProcessed;
    }
}