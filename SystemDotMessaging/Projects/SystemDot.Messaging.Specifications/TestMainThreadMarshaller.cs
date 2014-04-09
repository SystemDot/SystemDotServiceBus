using System;
using SystemDot.ThreadMarshalling;

namespace SystemDot.Messaging.Specifications
{
    public class TestMainThreadMarshaller : IMainThreadMarshaller
    {
        public bool WasRunThrough { private set; get; }

        public void RunOnMainThread(Action toRun)
        {
            WasRunThrough = true;
            toRun();
        }
    }
}