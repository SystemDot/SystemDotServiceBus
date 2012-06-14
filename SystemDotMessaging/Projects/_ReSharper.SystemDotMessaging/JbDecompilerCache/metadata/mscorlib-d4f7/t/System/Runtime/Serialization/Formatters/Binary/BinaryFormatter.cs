// Type: System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary
{
    [ComVisible(true)]
    public sealed class BinaryFormatter : IRemotingFormatter, IFormatter
    {
        public BinaryFormatter();
        public BinaryFormatter(ISurrogateSelector selector, StreamingContext context);
        public FormatterTypeStyle TypeFormat { get; set; }
        public FormatterAssemblyStyle AssemblyFormat { get; set; }
        public TypeFilterLevel FilterLevel { get; set; }

        #region IRemotingFormatter Members

        [SecuritySafeCritical]
        public object Deserialize(Stream serializationStream);

        [SecuritySafeCritical]
        public object Deserialize(Stream serializationStream, HeaderHandler handler);

        [SecuritySafeCritical]
        public void Serialize(Stream serializationStream, object graph);

        [SecuritySafeCritical]
        public void Serialize(Stream serializationStream, object graph, Header[] headers);

        public ISurrogateSelector SurrogateSelector { get; set; }
        public SerializationBinder Binder { get; set; }
        public StreamingContext Context { get; set; }

        #endregion

        [SecuritySafeCritical]
        public object DeserializeMethodResponse(Stream serializationStream, HeaderHandler handler, IMethodCallMessage methodCallMessage);

        [SecurityCritical]
        [ComVisible(false)]
        public object UnsafeDeserialize(Stream serializationStream, HeaderHandler handler);

        [SecurityCritical]
        [ComVisible(false)]
        public object UnsafeDeserializeMethodResponse(Stream serializationStream, HeaderHandler handler, IMethodCallMessage methodCallMessage);
    }
}
