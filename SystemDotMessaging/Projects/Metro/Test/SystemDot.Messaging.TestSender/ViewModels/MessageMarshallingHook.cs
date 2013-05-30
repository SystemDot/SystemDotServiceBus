using System;
using Windows.UI.Core;

namespace SystemDot.Messaging.TestSender.ViewModels
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

    public class MessageMarshallingHook2 : IMessageProcessor<object, object>
    {
        readonly CoreDispatcher dispatcher;

        public MessageMarshallingHook2(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public async void InputMessage(object toInput)
        {
           this.MessageProcessed(toInput);
        }

        public event Action<object> MessageProcessed;
    }
}