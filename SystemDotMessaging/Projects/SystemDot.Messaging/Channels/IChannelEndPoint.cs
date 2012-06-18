using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Channels
{
    [ContractClass(typeof(IChannelEndPointContract<>))]
    public interface IChannelEndPoint<in T>
    {
        void InputMessage(T toInput);
    }

    [ContractClassFor(typeof(IChannelEndPoint<>))]
    public class IChannelEndPointContract<T> : IChannelEndPoint<T> 
    {
        public void InputMessage(T toInput)
        {
            Contract.Requires(toInput != null);
        }
    }
}