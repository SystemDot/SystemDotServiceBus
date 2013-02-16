using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Transport
{
    [ContractClass(typeof(ITransportBuilderContract))]
    public interface ITransportBuilder
    {
        void Build(ServerPath toListenFor);
    }

    [ContractClassFor(typeof(ITransportBuilder))]
    public class ITransportBuilderContract
    {
        public void Build(ServerPath toListenFor)
        {
            Contract.Requires(toListenFor != null);
        }
    }
}