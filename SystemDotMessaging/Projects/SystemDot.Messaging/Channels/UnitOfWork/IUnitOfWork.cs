using System;

namespace SystemDot.Messaging.Channels.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Begin();

        void End(Exception ex = null);
    }
}