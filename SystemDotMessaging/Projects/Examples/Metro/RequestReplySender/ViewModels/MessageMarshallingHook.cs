using System;
using SystemDot.Messaging.Hooks;
using Windows.UI.Core;

namespace RequestReplySender.ViewModels
{
    public class MessageMarshallingHook : IMessageHook<object>
    {
        readonly CoreDispatcher dispatcher;

        public MessageMarshallingHook(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public async void ProcessMessage(object toInput, Action<object> toPerformOnOutput)
        {
            await this.dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => toPerformOnOutput(toInput));
        }
    }
}