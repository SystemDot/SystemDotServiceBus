using System;
using SystemDot.Messaging;
using Windows.UI.Core;

namespace RequestReplySender.ViewModels
{
    public class MessageMarshallingHook : IMessageProcessor<object, object>
    {
        readonly CoreDispatcher dispatcher;

        public MessageMarshallingHook(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public async void InputMessage(object toInput)
        {
            await this.dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.MessageProcessed(toInput));
        }

        public event Action<object> MessageProcessed;
    }
}