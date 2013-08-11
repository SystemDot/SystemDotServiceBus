using System;
using MonoTouch.UIKit;

namespace SystemDot
{
    public class MainThreadDispatcher
    {
        public void Dispatch(Action toDispatch)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() => toDispatch());
        }
    }
}