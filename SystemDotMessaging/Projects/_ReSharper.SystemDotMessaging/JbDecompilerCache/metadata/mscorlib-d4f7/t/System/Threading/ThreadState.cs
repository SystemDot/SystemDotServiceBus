// Type: System.Threading.ThreadState
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
    /// <summary>
    /// Specifies the execution states of a <see cref="T:System.Threading.Thread"/>.
    /// </summary>
    /// <filterpriority>1</filterpriority>
    [Flags]
    [ComVisible(true)]
    [Serializable]
    public enum ThreadState
    {
        Running = 0,
        StopRequested = 1,
        SuspendRequested = 2,
        Background = 4,
        Unstarted = 8,
        Stopped = 16,
        WaitSleepJoin = 32,
        Suspended = 64,
        AbortRequested = 128,
        Aborted = 256,
    }
}
