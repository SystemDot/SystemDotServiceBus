using System.Diagnostics.Contracts;

namespace SystemDot.Messaging
{
    [ContractClass(typeof(MessageInputterContract<>))]
    public interface IMessageInputter<in T>
    {
        void InputMessage(T toInput);
    }

    [ContractClassFor(typeof(IMessageInputter<>))]
    public class MessageInputterContract<T> : IMessageInputter<T>
    {
        public void InputMessage(T toInput)
        {
            Contract.Requires(toInput != null);
        }
    }
}