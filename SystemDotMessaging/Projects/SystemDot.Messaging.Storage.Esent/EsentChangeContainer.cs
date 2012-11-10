using System;

namespace SystemDot.Messaging.Storage.Esent
{
    [Serializable]
    public struct EsentChangeContainer
    {
        public int Sequence { get; set; }
        public string ChangeRootId { get; set; }
        public string Change { get; set; }
    }
}
