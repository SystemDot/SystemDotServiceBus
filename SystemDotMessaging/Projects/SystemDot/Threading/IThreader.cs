using System;

namespace SystemDot.Threading
{
    public interface IThreader
    {
        void Start(Action action);
    }
}