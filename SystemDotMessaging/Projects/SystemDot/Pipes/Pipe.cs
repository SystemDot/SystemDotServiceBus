using System;

namespace SystemDot.Pipes
{
    public class Pipe<T> : IPipe<T>
    {
        public event Action<T> ItemPushed;

        public void Push(T item)
        {
            ItemPushed.Invoke(item);
        }
    }
}