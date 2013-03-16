using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Transport
{
    [ContractClass(typeof(ITransportBuilderContract))]
    interface ITransportBuilder
    {
        void Build(ServerPath toListenFor);
    }

    [ContractClassFor(typeof(ITransportBuilder))]
    class ITransportBuilderContract
    {
        public void Build(ServerPath toListenFor)
        {
            Contract.Requires(toListenFor != null);
        }
    }
}