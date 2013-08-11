using System;
using System.Diagnostics.Contracts;

namespace SystemDot.ThreadMashalling
{
    [ContractClass(typeof(IMainThreadMarshallerContract))]
    public interface IMainThreadMarshaller
    {
        void RunOnMainThread(Action toRun);
    }

    [ContractClassFor(typeof(IMainThreadMarshaller))]
    public class IMainThreadMarshallerContract : IMainThreadMarshaller
    {
        public void RunOnMainThread(Action toRun)
        {
            Contract.Requires(toRun != null);
        }
    }
}