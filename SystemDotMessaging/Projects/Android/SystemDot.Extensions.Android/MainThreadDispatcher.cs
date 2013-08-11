using System;
using SystemDot.Configuration;
using Android.App;

namespace SystemDot
{
    public class MainThreadDispatcher
    {
        public void Dispatch(Action toDispatch)
        {
            MainActivityLocator.Locate().RunOnUiThread(toDispatch);
        }
    }
}