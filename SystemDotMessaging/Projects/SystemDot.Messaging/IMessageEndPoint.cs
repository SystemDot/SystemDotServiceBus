namespace SystemDot.Messaging
{
    public interface IMessageEndPoint<in T>
    {
        void InputMessage(T toInput);
    }
}