using System;

namespace SystemDot.Messaging.Threading
{
    public interface IThreader
    {
        void Start(Action action);
    }
}