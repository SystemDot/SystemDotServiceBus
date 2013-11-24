using System;

namespace SystemDot.Storage.Changes.Upcasting
{
    interface IUpcaster
    {
        Change Upcast(Change toUpcast);
        Type ChangeType { get; }
    }
}