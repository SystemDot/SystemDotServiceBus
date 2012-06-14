using System;

namespace SystemDot.Pipes
{
    public interface IPipe<T> 
    {
        event Action<T> ItemPushed;
        void Push(T item);
    }
}