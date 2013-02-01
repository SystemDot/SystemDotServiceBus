using System;

namespace SystemDot.Messaging.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Begin();

        void End(Exception ex = null);
    }
}