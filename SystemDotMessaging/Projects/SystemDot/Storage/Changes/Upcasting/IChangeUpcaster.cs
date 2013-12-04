using System;

namespace SystemDot.Storage.Changes.Upcasting
{
    public interface IChangeUpcaster
    {
        Change Upcast(Change toUpcast);
        Type ChangeType { get; }
        int Version { get; }
    }
}