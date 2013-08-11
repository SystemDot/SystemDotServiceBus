using System;
using Windows.UI.Core;

namespace SystemDot
{
    public class MainThreadDispatcher
    {
        readonly CoreDispatcher dispatcher;

        public MainThreadDispatcher()
        {
            this.dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
        }

        public async void Dispatch(Action toDispatch)
        {
            await this.dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => toDispatch());
        }
    }
}