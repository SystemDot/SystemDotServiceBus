using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Transport
{
    [ContractClass(typeof(ITransportBuilderContract))]
    interface ITransportBuilder
    {
        void Build(ServerRoute toListenFor);
    }

    [ContractClassFor(typeof(ITransportBuilder))]
    class ITransportBuilderContract
    {
        public void Build(ServerRoute toListenFor)
        {
            Contract.Requires(toListenFor != null);
        }
    }
}