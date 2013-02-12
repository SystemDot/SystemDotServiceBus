﻿using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Transport
{
    public interface ITransportBuilder
    {
        void Build(ServerPath toListenFor);
    }
}