using System;

namespace SystemDot.Messaging.Hooks.External
{
    public interface IExternalInspector
    {
        object ProcessMessage(object toInput);
        Type MessageType { get; }
    }
}