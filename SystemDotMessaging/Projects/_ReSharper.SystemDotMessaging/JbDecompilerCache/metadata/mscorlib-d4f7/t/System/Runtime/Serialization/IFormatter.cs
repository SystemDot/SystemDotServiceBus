// Type: System.Runtime.Serialization.IFormatter
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System.IO;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
    [ComVisible(true)]
    public interface IFormatter
    {
        ISurrogateSelector SurrogateSelector { get; set; }
        SerializationBinder Binder { get; set; }
        StreamingContext Context { get; set; }
        object Deserialize(Stream serializationStream);
        void Serialize(Stream serializationStream, object graph);
    }
}
