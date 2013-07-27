using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Transport
{
    [ContractClass(typeof(ITransportBuilderContract))]
    interface ITransportBuilder
    {
        void Build(MessageServer toListenFor);
    }

    [ContractClassFor(typeof(ITransportBuilder))]
    class ITransportBuilderContract
    {
        public void Build(MessageServer toListenFor)
        {
            Contract.Requires(toListenFor != null);
        }
    }
}