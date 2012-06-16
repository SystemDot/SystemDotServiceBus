// Type: System.Threading.Thread
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System;
using System.Globalization;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.Threading
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof (_Thread))]
    [ComVisible(true)]
    public sealed class Thread : CriticalFinalizerObject, _Thread
    {
        [SecuritySafeCritical]
        public Thread(ThreadStart start);

        [SecuritySafeCritical]
        public Thread(ThreadStart start, int maxStackSize);

        [SecuritySafeCritical]
        public Thread(ParameterizedThreadStart start);

        [SecuritySafeCritical]
        public Thread(ParameterizedThreadStart start, int maxStackSize);

        public int ManagedThreadId { [SecuritySafeCritical, ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), MethodImpl(MethodImplOptions.InternalCall)] get; }
        public ExecutionContext ExecutionContext { [SecuritySafeCritical, ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)] get; }
        public ThreadPriority Priority { [SecuritySafeCritical] get; [SecuritySafeCritical, HostProtection(SecurityAction.LinkDemand, SelfAffectingThreading = true)] set; }
        public bool IsAlive { [SecuritySafeCritical] get; }
        public bool IsThreadPoolThread { [SecuritySafeCritical] get; }
        public static Thread CurrentThread { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries"), SecuritySafeCritical, ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)] get; }
        public bool IsBackground { [SecuritySafeCritical] get; [SecuritySafeCritical, HostProtection(SecurityAction.LinkDemand, SelfAffectingThreading = true)] set; }
        public ThreadState ThreadState { [SecuritySafeCritical] get; }

        [Obsolete("The ApartmentState property has been deprecated.  Use GetApartmentState, SetApartmentState or TrySetApartmentState instead.", false)]
        public ApartmentState ApartmentState { [SecuritySafeCritical] get; [SecuritySafeCritical, HostProtection(SecurityAction.LinkDemand, SelfAffectingThreading = true, Synchronization = true)] set; }

        public CultureInfo CurrentUICulture { [SecuritySafeCritical] get; [SecuritySafeCritical, HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)] set; }
        public CultureInfo CurrentCulture { [SecuritySafeCritical] get; [SecuritySafeCritical, SecurityPermission(SecurityAction.Demand, ControlThread = true)] set; }
        public static Context CurrentContext { [SecurityCritical] get; }
        public static IPrincipal CurrentPrincipal { [SecuritySafeCritical] get; [SecuritySafeCritical, TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries"), SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)] set; }
        public string Name { get; [SecuritySafeCritical, HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)] set; }

        #region _Thread Members

        void _Thread.GetTypeInfoCount(out uint pcTInfo);
        void _Thread.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);
        void _Thread.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);
        void _Thread.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

        #endregion

        [ComVisible(false)]
        public override int GetHashCode();

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.NoInlining)]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public void Start();

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.NoInlining)]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public void Start(object parameter);

        [SecurityCritical]
        [Obsolete("Thread.SetCompressedStack is no longer supported. Please use the System.Threading.CompressedStack class")]
        public void SetCompressedStack(CompressedStack stack);

        [SecurityCritical]
        [Obsolete("Thread.GetCompressedStack is no longer supported. Please use the System.Threading.CompressedStack class")]
        public CompressedStack GetCompressedStack();

        [SecuritySafeCritical]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Abort(object stateInfo);

        [SecuritySafeCritical]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Abort();

        [SecuritySafeCritical]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public static void ResetAbort();

        [SecuritySafeCritical]
        [Obsolete("Thread.Suspend has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202", false)]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Suspend();

        [Obsolete("Thread.Resume has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202", false)]
        [SecuritySafeCritical]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Resume();

        [SecuritySafeCritical]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Interrupt();

        [SecuritySafeCritical]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public void Join();

        [SecuritySafeCritical]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public bool Join(int millisecondsTimeout);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public bool Join(TimeSpan timeout);

        [SecuritySafeCritical]
        public static void Sleep(int millisecondsTimeout);

        public static void Sleep(TimeSpan timeout);

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public static void SpinWait(int iterations);

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public static bool Yield();

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        ~Thread();

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public void DisableComObjectEagerCleanup();

        [SecuritySafeCritical]
        public ApartmentState GetApartmentState();

        [SecuritySafeCritical]
        [HostProtection(SecurityAction.LinkDemand, SelfAffectingThreading = true, Synchronization = true)]
        public bool TrySetApartmentState(ApartmentState state);

        [SecuritySafeCritical]
        [HostProtection(SecurityAction.LinkDemand, SelfAffectingThreading = true, Synchronization = true)]
        public void SetApartmentState(ApartmentState state);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, SharedState = true)]
        public static LocalDataStoreSlot AllocateDataSlot();

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, SharedState = true)]
        public static LocalDataStoreSlot AllocateNamedDataSlot(string name);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, SharedState = true)]
        public static LocalDataStoreSlot GetNamedDataSlot(string name);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, SharedState = true)]
        public static void FreeNamedDataSlot(string name);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, SharedState = true)]
        public static object GetData(LocalDataStoreSlot slot);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, SharedState = true)]
        public static void SetData(LocalDataStoreSlot slot, object data);

        [SecuritySafeCritical]
        public static AppDomain GetDomain();

        [SecuritySafeCritical]
        public static int GetDomainID();

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public static void BeginCriticalRegion();

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public static void EndCriticalRegion();

        [SecurityCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static void BeginThreadAffinity();

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static void EndThreadAffinity();

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static byte VolatileRead(ref byte address);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static short VolatileRead(ref short address);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int VolatileRead(ref int address);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static long VolatileRead(ref long address);

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static sbyte VolatileRead(ref sbyte address);

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ushort VolatileRead(ref ushort address);

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static uint VolatileRead(ref uint address);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IntPtr VolatileRead(ref IntPtr address);

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static UIntPtr VolatileRead(ref UIntPtr address);

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ulong VolatileRead(ref ulong address);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static float VolatileRead(ref float address);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static double VolatileRead(ref double address);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static object VolatileRead(ref object address);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref byte address, byte value);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref short address, short value);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref int address, int value);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref long address, long value);

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref sbyte address, sbyte value);

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref ushort address, ushort value);

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref uint address, uint value);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref IntPtr address, IntPtr value);

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref UIntPtr address, UIntPtr value);

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref ulong address, ulong value);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref float address, float value);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref double address, double value);

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref object address, object value);

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static void MemoryBarrier();
    }
}
