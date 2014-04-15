using System;
using System.ComponentModel.Composition;

namespace SystemDot.Messaging.Hooks.External
{
    [InheritedExport]
    public interface IExternalInspector
    {
        object ProcessMessage(object toInput);
        Type MessageType { get; }
    }
}