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
    /// <summary>
    /// Creates and controls a thread, sets its priority, and gets its status.
    /// </summary>
    /// <filterpriority>1</filterpriority>
    [ComDefaultInterface(typeof (_Thread))]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public sealed class Thread : CriticalFinalizerObject, _Thread
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Threading.Thread"/> class.
        /// </summary>
        /// <param name="start">A <see cref="T:System.Threading.ThreadStart"/> delegate that represents the methods to be invoked when this thread begins executing. </param><exception cref="T:System.ArgumentNullException">The <paramref name="start"/> parameter is null. </exception>
        [SecuritySafeCritical]
        public Thread(ThreadStart start);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Threading.Thread"/> class, specifying the maximum stack size for the thread.
        /// </summary>
        /// <param name="start">A <see cref="T:System.Threading.ThreadStart"/> delegate that represents the methods to be invoked when this thread begins executing.</param><param name="maxStackSize">The maximum stack size to be used by the thread, or 0 to use the default maximum stack size specified in the header for the executable.ImportantFor partially trusted code, <paramref name="maxStackSize"/> is ignored if it is greater than the default stack size. No exception is thrown. </param><exception cref="T:System.ArgumentNullException"><paramref name="start"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="maxStackSize"/> is less than zero.</exception>
        [SecuritySafeCritical]
        public Thread(ThreadStart start, int maxStackSize);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Threading.Thread"/> class, specifying a delegate that allows an object to be passed to the thread when the thread is started.
        /// </summary>
        /// <param name="start">A <see cref="T:System.Threading.ParameterizedThreadStart"/> delegate that represents the methods to be invoked when this thread begins executing.</param><exception cref="T:System.ArgumentNullException"><paramref name="start"/> is null. </exception>
        [SecuritySafeCritical]
        public Thread(ParameterizedThreadStart start);

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Threading.Thread"/> class, specifying a delegate that allows an object to be passed to the thread when the thread is started and specifying the maximum stack size for the thread.
        /// </summary>
        /// <param name="start">A <see cref="T:System.Threading.ParameterizedThreadStart"/> delegate that represents the methods to be invoked when this thread begins executing.</param><param name="maxStackSize">The maximum stack size to be used by the thread, or 0 to use the default maximum stack size specified in the header for the executable.ImportantFor partially trusted code, <paramref name="maxStackSize"/> is ignored if it is greater than the default stack size. No exception is thrown. </param><exception cref="T:System.ArgumentNullException"><paramref name="start"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="maxStackSize"/> is less than zero.</exception>
        [SecuritySafeCritical]
        public Thread(ParameterizedThreadStart start, int maxStackSize);

        /// <summary>
        /// Returns a hash code for the current thread.
        /// </summary>
        /// 
        /// <returns>
        /// An integer hash code value.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [ComVisible(false)]
        public override int GetHashCode();

        /// <summary>
        /// Causes the operating system to change the state of the current instance to <see cref="F:System.Threading.ThreadState.Running"/>.
        /// </summary>
        /// <exception cref="T:System.Threading.ThreadStateException">The thread has already been started. </exception><exception cref="T:System.OutOfMemoryException">There is not enough memory available to start this thread. </exception><filterpriority>1</filterpriority>
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.NoInlining)]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public void Start();

        /// <summary>
        /// Causes the operating system to change the state of the current instance to <see cref="F:System.Threading.ThreadState.Running"/>, and optionally supplies an object containing data to be used by the method the thread executes.
        /// </summary>
        /// <param name="parameter">An object that contains data to be used by the method the thread executes.</param><exception cref="T:System.Threading.ThreadStateException">The thread has already been started. </exception><exception cref="T:System.OutOfMemoryException">There is not enough memory available to start this thread. </exception><exception cref="T:System.InvalidOperationException">This thread was created using a <see cref="T:System.Threading.ThreadStart"/> delegate instead of a <see cref="T:System.Threading.ParameterizedThreadStart"/> delegate.</exception><filterpriority>1</filterpriority>
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.NoInlining)]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public void Start(object parameter);

        /// <summary>
        /// Applies a captured <see cref="T:System.Threading.CompressedStack"/> to the current thread.
        /// </summary>
        /// <param name="stack">The <see cref="T:System.Threading.CompressedStack"/> object to be applied to the current thread.</param><exception cref="T:System.InvalidOperationException">In all cases. </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/><IPermission class="System.Security.Permissions.StrongNameIdentityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PublicKeyBlob="00000000000000000400000000000000"/></PermissionSet>
        [SecurityCritical]
        [Obsolete("Thread.SetCompressedStack is no longer supported. Please use the System.Threading.CompressedStack class")]
        public void SetCompressedStack(CompressedStack stack);

        /// <summary>
        /// Returns a <see cref="T:System.Threading.CompressedStack"/> object that can be used to capture the stack for the current thread.
        /// </summary>
        /// 
        /// <returns>
        /// None.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">In all cases.</exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/><IPermission class="System.Security.Permissions.StrongNameIdentityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PublicKeyBlob="00000000000000000400000000000000"/></PermissionSet>
        [Obsolete("Thread.GetCompressedStack is no longer supported. Please use the System.Threading.CompressedStack class")]
        [SecurityCritical]
        public CompressedStack GetCompressedStack();

        /// <summary>
        /// Raises a <see cref="T:System.Threading.ThreadAbortException"/> in the thread on which it is invoked, to begin the process of terminating the thread while also providing exception information about the thread termination. Calling this method usually terminates the thread.
        /// </summary>
        /// <param name="stateInfo">An object that contains application-specific information, such as state, which can be used by the thread being aborted. </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><exception cref="T:System.Threading.ThreadStateException">The thread that is being aborted is currently suspended.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlThread"/></PermissionSet>
        [SecuritySafeCritical]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Abort(object stateInfo);

        /// <summary>
        /// Raises a <see cref="T:System.Threading.ThreadAbortException"/> in the thread on which it is invoked, to begin the process of terminating the thread. Calling this method usually terminates the thread.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><exception cref="T:System.Threading.ThreadStateException">The thread that is being aborted is currently suspended.</exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlThread"/></PermissionSet>
        [SecuritySafeCritical]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Abort();

        /// <summary>
        /// Cancels an <see cref="M:System.Threading.Thread.Abort(System.Object)"/> requested for the current thread.
        /// </summary>
        /// <exception cref="T:System.Threading.ThreadStateException">Abort was not invoked on the current thread. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the required security permission for the current thread. </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlThread"/></PermissionSet>
        [SecuritySafeCritical]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public static void ResetAbort();

        /// <summary>
        /// Either suspends the thread, or if the thread is already suspended, has no effect.
        /// </summary>
        /// <exception cref="T:System.Threading.ThreadStateException">The thread has not been started or is dead. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the appropriate <see cref="T:System.Security.Permissions.SecurityPermission"/>. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlThread"/></PermissionSet>
        [SecuritySafeCritical]
        [Obsolete("Thread.Suspend has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202", false)]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Suspend();

        /// <summary>
        /// Resumes a thread that has been suspended.
        /// </summary>
        /// <exception cref="T:System.Threading.ThreadStateException">The thread has not been started, is dead, or is not in the suspended state. </exception><exception cref="T:System.Security.SecurityException">The caller does not have the appropriate <see cref="T:System.Security.Permissions.SecurityPermission"/>. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlThread"/></PermissionSet>
        [Obsolete("Thread.Resume has been deprecated.  Please use other classes in System.Threading, such as Monitor, Mutex, Event, and Semaphore, to synchronize Threads or protect resources.  http://go.microsoft.com/fwlink/?linkid=14202", false)]
        [SecuritySafeCritical]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Resume();

        /// <summary>
        /// Interrupts a thread that is in the WaitSleepJoin thread state.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the appropriate <see cref="T:System.Security.Permissions.SecurityPermission"/>. </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlThread"/></PermissionSet>
        [SecuritySafeCritical]
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void Interrupt();

        /// <summary>
        /// Blocks the calling thread until a thread terminates, while continuing to perform standard COM and SendMessage pumping.
        /// </summary>
        /// <exception cref="T:System.Threading.ThreadStateException">The caller attempted to join a thread that is in the <see cref="F:System.Threading.ThreadState.Unstarted"/> state. </exception><exception cref="T:System.Threading.ThreadInterruptedException">The thread is interrupted while waiting. </exception><filterpriority>1</filterpriority>
        [SecuritySafeCritical]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public void Join();

        /// <summary>
        /// Blocks the calling thread until a thread terminates or the specified time elapses, while continuing to perform standard COM and SendMessage pumping.
        /// </summary>
        /// 
        /// <returns>
        /// true if the thread has terminated; false if the thread has not terminated after the amount of time specified by the <paramref name="millisecondsTimeout"/> parameter has elapsed.
        /// </returns>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait for the thread to terminate. </param><exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="millisecondsTimeout"/> is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite"/> in milliseconds. </exception><exception cref="T:System.Threading.ThreadStateException">The thread has not been started. </exception><filterpriority>1</filterpriority>
        [SecuritySafeCritical]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public bool Join(int millisecondsTimeout);

        /// <summary>
        /// Blocks the calling thread until a thread terminates or the specified time elapses, while continuing to perform standard COM and SendMessage pumping.
        /// </summary>
        /// 
        /// <returns>
        /// true if the thread terminated; false if the thread has not terminated after the amount of time specified by the <paramref name="timeout"/> parameter has elapsed.
        /// </returns>
        /// <param name="timeout">A <see cref="T:System.TimeSpan"/> set to the amount of time to wait for the thread to terminate. </param><exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="timeout"/> is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite"/> in milliseconds, or is greater than <see cref="F:System.Int32.MaxValue"/> milliseconds. </exception><exception cref="T:System.Threading.ThreadStateException">The caller attempted to join a thread that is in the <see cref="F:System.Threading.ThreadState.Unstarted"/> state. </exception><filterpriority>1</filterpriority>
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public bool Join(TimeSpan timeout);

        /// <summary>
        /// Suspends the current thread for a specified time.
        /// </summary>
        /// <param name="millisecondsTimeout">The number of milliseconds for which the thread is blocked. Specify zero (0) to indicate that this thread should be suspended to allow other waiting threads to execute. Specify <see cref="F:System.Threading.Timeout.Infinite"/> to block the thread indefinitely. </param><exception cref="T:System.ArgumentOutOfRangeException">The time-out value is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite"/>. </exception><filterpriority>1</filterpriority>
        [SecuritySafeCritical]
        public static void Sleep(int millisecondsTimeout);

        /// <summary>
        /// Blocks the current thread for a specified time.
        /// </summary>
        /// <param name="timeout">A <see cref="T:System.TimeSpan"/> set to the amount of time for which the thread is blocked. Specify zero to indicate that this thread should be suspended to allow other waiting threads to execute. Specify <see cref="F:System.Threading.Timeout.Infinite"/> to block the thread indefinitely. </param><exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="timeout"/> is negative and is not equal to <see cref="F:System.Threading.Timeout.Infinite"/> in milliseconds, or is greater than <see cref="F:System.Int32.MaxValue"/> milliseconds. </exception><filterpriority>1</filterpriority>
        public static void Sleep(TimeSpan timeout);

        /// <summary>
        /// Causes a thread to wait the number of times defined by the <paramref name="iterations"/> parameter.
        /// </summary>
        /// <param name="iterations">A 32-bit signed integer that defines how long a thread is to wait. </param><filterpriority>1</filterpriority>
        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public static void SpinWait(int iterations);

        /// <summary>
        /// Causes the calling thread to yield execution to another thread that is ready to run on the current processor. The operating system selects the thread to yield to.
        /// </summary>
        /// 
        /// <returns>
        /// true if the operating system switched execution to another thread; otherwise, false.
        /// </returns>
        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public static bool Yield();

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        ~Thread();

        /// <summary>
        /// Turns off automatic cleanup of runtime callable wrappers (RCW) for the current thread.
        /// </summary>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public void DisableComObjectEagerCleanup();

        /// <summary>
        /// Returns an <see cref="T:System.Threading.ApartmentState"/> value indicating the apartment state.
        /// </summary>
        /// 
        /// <returns>
        /// One of the <see cref="T:System.Threading.ApartmentState"/> values indicating the apartment state of the managed thread. The default is <see cref="F:System.Threading.ApartmentState.Unknown"/>.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        [SecuritySafeCritical]
        public ApartmentState GetApartmentState();

        /// <summary>
        /// Sets the apartment state of a thread before it is started.
        /// </summary>
        /// 
        /// <returns>
        /// true if the apartment state is set; otherwise, false.
        /// </returns>
        /// <param name="state">The new apartment state.</param><exception cref="T:System.ArgumentException"><paramref name="state"/> is not a valid apartment state.</exception><exception cref="T:System.Threading.ThreadStateException">The thread has already been started.</exception><filterpriority>1</filterpriority>
        [SecuritySafeCritical]
        [HostProtection(SecurityAction.LinkDemand, SelfAffectingThreading = true, Synchronization = true)]
        public bool TrySetApartmentState(ApartmentState state);

        /// <summary>
        /// Sets the apartment state of a thread before it is started.
        /// </summary>
        /// <param name="state">The new apartment state.</param><exception cref="T:System.ArgumentException"><paramref name="state"/> is not a valid apartment state.</exception><exception cref="T:System.Threading.ThreadStateException">The thread has already been started.</exception><exception cref="T:System.InvalidOperationException">The apartment state has already been initialized.</exception><filterpriority>1</filterpriority>
        [SecuritySafeCritical]
        [HostProtection(SecurityAction.LinkDemand, SelfAffectingThreading = true, Synchronization = true)]
        public void SetApartmentState(ApartmentState state);

        /// <summary>
        /// Allocates an unnamed data slot on all the threads. For better performance, use fields that are marked with the <see cref="T:System.ThreadStaticAttribute"/> attribute instead.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.LocalDataStoreSlot"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, SharedState = true)]
        public static LocalDataStoreSlot AllocateDataSlot();

        /// <summary>
        /// Allocates a named data slot on all threads. For better performance, use fields that are marked with the <see cref="T:System.ThreadStaticAttribute"/> attribute instead.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.LocalDataStoreSlot"/>.
        /// </returns>
        /// <param name="name">The name of the data slot to be allocated. </param><exception cref="T:System.ArgumentException">A named data slot with the specified name already exists.</exception><filterpriority>2</filterpriority>
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, SharedState = true)]
        public static LocalDataStoreSlot AllocateNamedDataSlot(string name);

        /// <summary>
        /// Looks up a named data slot. For better performance, use fields that are marked with the <see cref="T:System.ThreadStaticAttribute"/> attribute instead.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.LocalDataStoreSlot"/> allocated for this thread.
        /// </returns>
        /// <param name="name">The name of the local data slot. </param><filterpriority>2</filterpriority>
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, SharedState = true)]
        public static LocalDataStoreSlot GetNamedDataSlot(string name);

        /// <summary>
        /// Eliminates the association between a name and a slot, for all threads in the process. For better performance, use fields that are marked with the <see cref="T:System.ThreadStaticAttribute"/> attribute instead.
        /// </summary>
        /// <param name="name">The name of the data slot to be freed. </param><filterpriority>2</filterpriority>
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, SharedState = true)]
        public static void FreeNamedDataSlot(string name);

        /// <summary>
        /// Retrieves the value from the specified slot on the current thread, within the current thread's current domain. For better performance, use fields that are marked with the <see cref="T:System.ThreadStaticAttribute"/> attribute instead.
        /// </summary>
        /// 
        /// <returns>
        /// The retrieved value.
        /// </returns>
        /// <param name="slot">The <see cref="T:System.LocalDataStoreSlot"/> from which to get the value. </param><filterpriority>2</filterpriority>
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, SharedState = true)]
        public static object GetData(LocalDataStoreSlot slot);

        /// <summary>
        /// Sets the data in the specified slot on the currently running thread, for that thread's current domain. For better performance, use fields marked with the <see cref="T:System.ThreadStaticAttribute"/> attribute instead.
        /// </summary>
        /// <param name="slot">The <see cref="T:System.LocalDataStoreSlot"/> in which to set the value. </param><param name="data">The value to be set. </param><filterpriority>1</filterpriority>
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, SharedState = true)]
        public static void SetData(LocalDataStoreSlot slot, object data);

        /// <summary>
        /// Returns the current domain in which the current thread is running.
        /// </summary>
        /// 
        /// <returns>
        /// An <see cref="T:System.AppDomain"/> representing the current application domain of the running thread.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [SecuritySafeCritical]
        public static AppDomain GetDomain();

        /// <summary>
        /// Returns a unique application domain identifier.
        /// </summary>
        /// 
        /// <returns>
        /// A 32-bit signed integer uniquely identifying the application domain.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [SecuritySafeCritical]
        public static int GetDomainID();

        /// <summary>
        /// Notifies a host that execution is about to enter a region of code in which the effects of a thread abort or unhandled exception might jeopardize other tasks in the application domain.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public static void BeginCriticalRegion();

        /// <summary>
        /// Notifies a host that execution is about to enter a region of code in which the effects of a thread abort or unhandled exception are limited to the current task.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.InternalCall)]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true, Synchronization = true)]
        public static void EndCriticalRegion();

        /// <summary>
        /// Notifies a host that managed code is about to execute instructions that depend on the identity of the current physical operating system thread.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlThread"/></PermissionSet>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static void BeginThreadAffinity();

        /// <summary>
        /// Notifies a host that managed code has finished executing instructions that depend on the identity of the current physical operating system thread.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlThread"/></PermissionSet>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static void EndThreadAffinity();

        /// <summary>
        /// Reads the value of a field. The value is the latest written by any processor in a computer, regardless of the number of processors or the state of processor cache.
        /// </summary>
        /// 
        /// <returns>
        /// The latest value written to the field by any processor.
        /// </returns>
        /// <param name="address">The field to be read. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static byte VolatileRead(ref byte address);

        /// <summary>
        /// Reads the value of a field. The value is the latest written by any processor in a computer, regardless of the number of processors or the state of processor cache.
        /// </summary>
        /// 
        /// <returns>
        /// The latest value written to the field by any processor.
        /// </returns>
        /// <param name="address">The field to be read. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static short VolatileRead(ref short address);

        /// <summary>
        /// Reads the value of a field. The value is the latest written by any processor in a computer, regardless of the number of processors or the state of processor cache.
        /// </summary>
        /// 
        /// <returns>
        /// The latest value written to the field by any processor.
        /// </returns>
        /// <param name="address">The field to be read. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int VolatileRead(ref int address);

        /// <summary>
        /// Reads the value of a field. The value is the latest written by any processor in a computer, regardless of the number of processors or the state of processor cache.
        /// </summary>
        /// 
        /// <returns>
        /// The latest value written to the field by any processor.
        /// </returns>
        /// <param name="address">The field to be read. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static long VolatileRead(ref long address);

        /// <summary>
        /// Reads the value of a field. The value is the latest written by any processor in a computer, regardless of the number of processors or the state of processor cache.
        /// </summary>
        /// 
        /// <returns>
        /// The latest value written to the field by any processor.
        /// </returns>
        /// <param name="address">The field to be read. </param><filterpriority>1</filterpriority>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static sbyte VolatileRead(ref sbyte address);

        /// <summary>
        /// Reads the value of a field. The value is the latest written by any processor in a computer, regardless of the number of processors or the state of processor cache.
        /// </summary>
        /// 
        /// <returns>
        /// The latest value written to the field by any processor.
        /// </returns>
        /// <param name="address">The field to be read. </param><filterpriority>1</filterpriority>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ushort VolatileRead(ref ushort address);

        /// <summary>
        /// Reads the value of a field. The value is the latest written by any processor in a computer, regardless of the number of processors or the state of processor cache.
        /// </summary>
        /// 
        /// <returns>
        /// The latest value written to the field by any processor.
        /// </returns>
        /// <param name="address">The field to be read. </param><filterpriority>1</filterpriority>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static uint VolatileRead(ref uint address);

        /// <summary>
        /// Reads the value of a field. The value is the latest written by any processor in a computer, regardless of the number of processors or the state of processor cache.
        /// </summary>
        /// 
        /// <returns>
        /// The latest value written to the field by any processor.
        /// </returns>
        /// <param name="address">The field to be read. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IntPtr VolatileRead(ref IntPtr address);

        /// <summary>
        /// Reads the value of a field. The value is the latest written by any processor in a computer, regardless of the number of processors or the state of processor cache.
        /// </summary>
        /// 
        /// <returns>
        /// The latest value written to the field by any processor.
        /// </returns>
        /// <param name="address">The field to be read. </param><filterpriority>1</filterpriority>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static UIntPtr VolatileRead(ref UIntPtr address);

        /// <summary>
        /// Reads the value of a field. The value is the latest written by any processor in a computer, regardless of the number of processors or the state of processor cache.
        /// </summary>
        /// 
        /// <returns>
        /// The latest value written to the field by any processor.
        /// </returns>
        /// <param name="address">The field to be read. </param><filterpriority>1</filterpriority>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ulong VolatileRead(ref ulong address);

        /// <summary>
        /// Reads the value of a field. The value is the latest written by any processor in a computer, regardless of the number of processors or the state of processor cache.
        /// </summary>
        /// 
        /// <returns>
        /// The latest value written to the field by any processor.
        /// </returns>
        /// <param name="address">The field to be read. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static float VolatileRead(ref float address);

        /// <summary>
        /// Reads the value of a field. The value is the latest written by any processor in a computer, regardless of the number of processors or the state of processor cache.
        /// </summary>
        /// 
        /// <returns>
        /// The latest value written to the field by any processor.
        /// </returns>
        /// <param name="address">The field to be read. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static double VolatileRead(ref double address);

        /// <summary>
        /// Reads the value of a field. The value is the latest written by any processor in a computer, regardless of the number of processors or the state of processor cache.
        /// </summary>
        /// 
        /// <returns>
        /// The latest value written to the field by any processor.
        /// </returns>
        /// <param name="address">The field to be read. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static object VolatileRead(ref object address);

        /// <summary>
        /// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
        /// </summary>
        /// <param name="address">The field to which the value is to be written. </param><param name="value">The value to be written. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref byte address, byte value);

        /// <summary>
        /// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
        /// </summary>
        /// <param name="address">The field to which the value is to be written. </param><param name="value">The value to be written. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref short address, short value);

        /// <summary>
        /// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
        /// </summary>
        /// <param name="address">The field to which the value is to be written. </param><param name="value">The value to be written. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref int address, int value);

        /// <summary>
        /// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
        /// </summary>
        /// <param name="address">The field to which the value is to be written. </param><param name="value">The value to be written. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref long address, long value);

        /// <summary>
        /// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
        /// </summary>
        /// <param name="address">The field to which the value is to be written. </param><param name="value">The value to be written. </param><filterpriority>1</filterpriority>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref sbyte address, sbyte value);

        /// <summary>
        /// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
        /// </summary>
        /// <param name="address">The field to which the value is to be written. </param><param name="value">The value to be written. </param><filterpriority>1</filterpriority>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref ushort address, ushort value);

        /// <summary>
        /// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
        /// </summary>
        /// <param name="address">The field to which the value is to be written. </param><param name="value">The value to be written. </param><filterpriority>1</filterpriority>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref uint address, uint value);

        /// <summary>
        /// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
        /// </summary>
        /// <param name="address">The field to which the value is to be written. </param><param name="value">The value to be written. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref IntPtr address, IntPtr value);

        /// <summary>
        /// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
        /// </summary>
        /// <param name="address">The field to which the value is to be written. </param><param name="value">The value to be written. </param><filterpriority>1</filterpriority>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref UIntPtr address, UIntPtr value);

        /// <summary>
        /// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
        /// </summary>
        /// <param name="address">The field to which the value is to be written. </param><param name="value">The value to be written. </param><filterpriority>1</filterpriority>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref ulong address, ulong value);

        /// <summary>
        /// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
        /// </summary>
        /// <param name="address">The field to which the value is to be written. </param><param name="value">The value to be written. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref float address, float value);

        /// <summary>
        /// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
        /// </summary>
        /// <param name="address">The field to which the value is to be written. </param><param name="value">The value to be written. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref double address, double value);

        /// <summary>
        /// Writes a value to a field immediately, so that the value is visible to all processors in the computer.
        /// </summary>
        /// <param name="address">The field to which the value is to be written. </param><param name="value">The value to be written. </param><filterpriority>1</filterpriority>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void VolatileWrite(ref object address, object value);

        /// <summary>
        /// Synchronizes memory access as follows: The processor executing the current thread cannot reorder instructions in such a way that memory accesses prior to the call to <see cref="M:System.Threading.Thread.MemoryBarrier"/> execute after memory accesses that follow the call to <see cref="M:System.Threading.Thread.MemoryBarrier"/>.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static void MemoryBarrier();

        /// <summary>
        /// Retrieves the number of type information interfaces that an object provides (either 0 or 1).
        /// </summary>
        /// <param name="pcTInfo">Points to a location that receives the number of type information interfaces provided by the object.</param><exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
        void _Thread.GetTypeInfoCount(out uint pcTInfo);

        /// <summary>
        /// Retrieves the type information for an object, which can then be used to get the type information for an interface.
        /// </summary>
        /// <param name="iTInfo">The type information to return.</param><param name="lcid">The locale identifier for the type information.</param><param name="ppTInfo">Receives a pointer to the requested type information object.</param><exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
        void _Thread.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

        /// <summary>
        /// Maps a set of names to a corresponding set of dispatch identifiers.
        /// </summary>
        /// <param name="riid">Reserved for future use. Must be IID_NULL.</param><param name="rgszNames">Passed-in array of names to be mapped.</param><param name="cNames">Count of the names to be mapped.</param><param name="lcid">The locale context in which to interpret the names.</param><param name="rgDispId">Caller-allocated array which receives the IDs corresponding to the names.</param><exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
        void _Thread.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

        /// <summary>
        /// Provides access to properties and methods exposed by an object.
        /// </summary>
        /// <param name="dispIdMember">Identifies the member.</param><param name="riid">Reserved for future use. Must be IID_NULL.</param><param name="lcid">The locale context in which to interpret arguments.</param><param name="wFlags">Flags describing the context of the call.</param><param name="pDispParams">Pointer to a structure containing an array of arguments, an array of argument DISPIDs for named arguments, and counts for the number of elements in the arrays.</param><param name="pVarResult">Pointer to the location where the result is to be stored.</param><param name="pExcepInfo">Pointer to a structure that contains exception information.</param><param name="puArgErr">The index of the first argument that has an error.</param><exception cref="T:System.NotImplementedException">Late-bound access using the COM IDispatch interface is not supported.</exception>
        void _Thread.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

        /// <summary>
        /// Gets a unique identifier for the current managed thread.
        /// </summary>
        /// 
        /// <returns>
        /// An integer that represents a unique identifier for this managed thread.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public int ManagedThreadId { [SecuritySafeCritical, ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), MethodImpl(MethodImplOptions.InternalCall)]
        get; }

        /// <summary>
        /// Gets an <see cref="T:System.Threading.ExecutionContext"/> object that contains information about the various contexts of the current thread.
        /// </summary>
        /// 
        /// <returns>
        /// An <see cref="T:System.Threading.ExecutionContext"/> object that consolidates context information for the current thread.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public ExecutionContext ExecutionContext { [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), SecuritySafeCritical]
        get; }

        /// <summary>
        /// Gets or sets a value indicating the scheduling priority of a thread.
        /// </summary>
        /// 
        /// <returns>
        /// One of the <see cref="T:System.Threading.ThreadPriority"/> values. The default value is Normal.
        /// </returns>
        /// <exception cref="T:System.Threading.ThreadStateException">The thread has reached a final state, such as <see cref="F:System.Threading.ThreadState.Aborted"/>. </exception><exception cref="T:System.ArgumentException">The value specified for a set operation is not a valid ThreadPriority value. </exception><filterpriority>1</filterpriority>
        public ThreadPriority Priority { [SecuritySafeCritical]
        get; [SecuritySafeCritical, HostProtection(SecurityAction.LinkDemand, SelfAffectingThreading = true)]
        set; }

        /// <summary>
        /// Gets a value indicating the execution status of the current thread.
        /// </summary>
        /// 
        /// <returns>
        /// true if this thread has been started and has not terminated normally or aborted; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public bool IsAlive { [SecuritySafeCritical]
        get; }

        /// <summary>
        /// Gets a value indicating whether or not a thread belongs to the managed thread pool.
        /// </summary>
        /// 
        /// <returns>
        /// true if this thread belongs to the managed thread pool; otherwise, false.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public bool IsThreadPoolThread { [SecuritySafeCritical]
        get; }

        /// <summary>
        /// Gets the currently running thread.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.Threading.Thread"/> that is the representation of the currently running thread.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public static Thread CurrentThread { [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries"), SecuritySafeCritical, ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        get; }

        /// <summary>
        /// Gets or sets a value indicating whether or not a thread is a background thread.
        /// </summary>
        /// 
        /// <returns>
        /// true if this thread is or is to become a background thread; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.Threading.ThreadStateException">The thread is dead. </exception><filterpriority>1</filterpriority>
        public bool IsBackground { [SecuritySafeCritical]
        get; [SecuritySafeCritical, HostProtection(SecurityAction.LinkDemand, SelfAffectingThreading = true)]
        set; }

        /// <summary>
        /// Gets a value containing the states of the current thread.
        /// </summary>
        /// 
        /// <returns>
        /// One of the <see cref="T:System.Threading.ThreadState"/> values indicating the state of the current thread. The initial value is Unstarted.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public ThreadState ThreadState { [SecuritySafeCritical]
        get; }

        /// <summary>
        /// Gets or sets the apartment state of this thread.
        /// </summary>
        /// 
        /// <returns>
        /// One of the <see cref="T:System.Threading.ApartmentState"/> values. The initial value is Unknown.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">An attempt is made to set this property to a state that is not a valid apartment state (a state other than single-threaded apartment (STA) or multithreaded apartment (MTA)). </exception><filterpriority>2</filterpriority>
        [Obsolete("The ApartmentState property has been deprecated.  Use GetApartmentState, SetApartmentState or TrySetApartmentState instead.", false)]
        public ApartmentState ApartmentState { [SecuritySafeCritical]
        get; [SecuritySafeCritical, HostProtection(SecurityAction.LinkDemand, SelfAffectingThreading = true, Synchronization = true)]
        set; }

        /// <summary>
        /// Gets or sets the current culture used by the Resource Manager to look up culture-specific resources at run time.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.Globalization.CultureInfo"/> representing the current culture.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The property is set to null. </exception><exception cref="T:System.ArgumentException">The property is set to a culture name that cannot be used to locate a resource file. Resource filenames must include only letters, numbers, hyphens or underscores.</exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode"/></PermissionSet>
        public CultureInfo CurrentUICulture { [SecuritySafeCritical]
        get; [SecuritySafeCritical, HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        set; }

        /// <summary>
        /// Gets or sets the culture for the current thread.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.Globalization.CultureInfo"/> representing the culture for the current thread.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The property is set to a neutral culture. Neutral cultures cannot be used in formatting and parsing and therefore cannot be set as the thread's current culture.</exception><exception cref="T:System.ArgumentNullException">The property is set to null.</exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlThread"/></PermissionSet>
        public CultureInfo CurrentCulture { [SecuritySafeCritical]
        get; [SecuritySafeCritical, SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        set; }

        /// <summary>
        /// Gets the current context in which the thread is executing.
        /// </summary>
        /// 
        /// <returns>
        /// A <see cref="T:System.Runtime.Remoting.Contexts.Context"/> representing the current thread context.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/></PermissionSet>
        public static Context CurrentContext { [SecurityCritical]
        get; }

        /// <summary>
        /// Gets or sets the thread's current principal (for role-based security).
        /// </summary>
        /// 
        /// <returns>
        /// An <see cref="T:System.Security.Principal.IPrincipal"/> value representing the security context.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the permission required to set the principal. </exception><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlPrincipal"/></PermissionSet>
        public static IPrincipal CurrentPrincipal { [SecuritySafeCritical]
        get; [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries"), SecuritySafeCritical, SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlPrincipal)]
        set; }

        /// <summary>
        /// Gets or sets the name of the thread.
        /// </summary>
        /// 
        /// <returns>
        /// A string containing the name of the thread, or null if no name was set.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">A set operation was requested, and the Name property has already been set. </exception><filterpriority>1</filterpriority>
        public string Name { get; [SecuritySafeCritical, HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        set; }
    }
}
