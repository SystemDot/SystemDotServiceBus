using System;
using System.Windows.Threading;

namespace SystemDot
{
    public class MainThreadDispatcher
    {
        public void Dispatch(Action toDispatch)
        {
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Normal, toDispatch);
        }
    }
}