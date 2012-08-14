using System;
using SystemDot.Messaging.Messages;
using Windows.UI.Core;

namespace SystemDot.Messaging.TestSubscriber.ViewModels
{
    public class MessageMarshallingHook : IMessageProcessor<object, object>
    {
        public void InputMessage(object toInput)
        {
            CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => MessageProcessed(toInput));
        }

        public event Action<object> MessageProcessed;
    }
}