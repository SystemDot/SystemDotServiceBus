using System;
using SystemDot.Logging;

namespace SystemDot.ThreadMashalling
{
    public class MainThreadMarshaller : IMainThreadMarshaller
    {
        readonly MainThreadDispatcher dispatcher;

        public MainThreadMarshaller(MainThreadDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public void RunOnMainThread(Action toRun) 
        {
            dispatcher.Dispatch(() => RunWithLogging(toRun));
        }

        public void RunWithLogging(Action toRun)
        {
            try
            {
                toRun();
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }
    }
}