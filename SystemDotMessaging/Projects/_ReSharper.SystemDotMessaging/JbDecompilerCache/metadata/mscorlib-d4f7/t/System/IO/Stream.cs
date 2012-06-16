// Type: System.IO.Stream
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.IO
{
    /// <summary>
    /// Provides a generic view of a sequence of bytes.
    /// </summary>
    /// <filterpriority>2</filterpriority>
    [ComVisible(true)]
    [Serializable]
    public abstract class Stream : MarshalByRefObject, IDisposable
    {
        /// <summary>
        /// A Stream with no backing store.
        /// </summary>
        /// <filterpriority>1</filterpriority>
        public static readonly Stream Null;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.Stream"/> class.
        /// </summary>
        protected Stream();

        /// <summary>
        /// Reads all the bytes from the current stream and writes them to the destination stream.
        /// </summary>
        /// <param name="destination">The stream that will contain the contents of the current stream.</param><exception cref="T:System.ArgumentNullException"><paramref name="destination"/> is null.</exception><exception cref="T:System.NotSupportedException">The current stream does not support reading.-or-<paramref name="destination"/> does not support writing.</exception><exception cref="T:System.ObjectDisposedException">Either the current stream or <paramref name="destination"/> were closed before the <see cref="M:System.IO.Stream.CopyTo(System.IO.Stream)"/> method was called.</exception><exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void CopyTo(Stream destination);

        /// <summary>
        /// Reads all the bytes from the current stream and writes them to a destination stream, using a specified buffer size.
        /// </summary>
        /// <param name="destination">The stream that will contain the contents of the current stream.</param><param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 4096.</param><exception cref="T:System.ArgumentNullException"><paramref name="destination"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="bufferSize"/> is negative or zero.</exception><exception cref="T:System.NotSupportedException">The current stream does not support reading.-or-<paramref name="destination"/> does not support writing.</exception><exception cref="T:System.ObjectDisposedException">Either the current stream or <paramref name="destination"/> were closed before the <see cref="M:System.IO.Stream.CopyTo(System.IO.Stream)"/> method was called.</exception><exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        public void CopyTo(Stream destination, int bufferSize);

        /// <summary>
        /// Closes the current stream and releases any resources (such as sockets and file handles) associated with the current stream.
        /// </summary>
        /// <filterpriority>1</filterpriority>
        public virtual void Close();

        /// <summary>
        /// Releases all resources used by the <see cref="T:System.IO.Stream"/>.
        /// </summary>
        public void Dispose();

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.Stream"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing);

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception><filterpriority>2</filterpriority>
        public abstract void Flush();

        /// <summary>
        /// Allocates a <see cref="T:System.Threading.WaitHandle"/> object.
        /// </summary>
        /// 
        /// <returns>
        /// A reference to the allocated WaitHandle.
        /// </returns>
        [Obsolete("CreateWaitHandle will be removed eventually.  Please use \"new ManualResetEvent(false)\" instead.")]
        protected virtual WaitHandle CreateWaitHandle();

        /// <summary>
        /// Begins an asynchronous read operation.
        /// </summary>
        /// 
        /// <returns>
        /// An <see cref="T:System.IAsyncResult"/> that represents the asynchronous read, which could still be pending.
        /// </returns>
        /// <param name="buffer">The buffer to read the data into. </param><param name="offset">The byte offset in <paramref name="buffer"/> at which to begin writing data read from the stream. </param><param name="count">The maximum number of bytes to read. </param><param name="callback">An optional asynchronous callback, to be called when the read is complete. </param><param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests. </param><exception cref="T:System.IO.IOException">Attempted an asynchronous read past the end of the stream, or a disk error occurs. </exception><exception cref="T:System.ArgumentException">One or more of the arguments is invalid. </exception><exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception><exception cref="T:System.NotSupportedException">The current Stream implementation does not support the read operation. </exception><filterpriority>2</filterpriority>
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public virtual IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state);

        /// <summary>
        /// Waits for the pending asynchronous read to complete.
        /// </summary>
        /// 
        /// <returns>
        /// The number of bytes read from the stream, between zero (0) and the number of bytes you requested. Streams return zero (0) only at the end of the stream, otherwise, they should block until at least one byte is available.
        /// </returns>
        /// <param name="asyncResult">The reference to the pending asynchronous request to finish. </param><exception cref="T:System.ArgumentNullException"><paramref name="asyncResult"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="asyncResult"/> did not originate from a <see cref="M:System.IO.Stream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)"/> method on the current stream. </exception><exception cref="T:System.IO.IOException">The stream is closed or an internal error has occurred.</exception><filterpriority>2</filterpriority>
        public virtual int EndRead(IAsyncResult asyncResult);

        /// <summary>
        /// Begins an asynchronous write operation.
        /// </summary>
        /// 
        /// <returns>
        /// An IAsyncResult that represents the asynchronous write, which could still be pending.
        /// </returns>
        /// <param name="buffer">The buffer to write data from. </param><param name="offset">The byte offset in <paramref name="buffer"/> from which to begin writing. </param><param name="count">The maximum number of bytes to write. </param><param name="callback">An optional asynchronous callback, to be called when the write is complete. </param><param name="state">A user-provided object that distinguishes this particular asynchronous write request from other requests. </param><exception cref="T:System.IO.IOException">Attempted an asynchronous write past the end of the stream, or a disk error occurs. </exception><exception cref="T:System.ArgumentException">One or more of the arguments is invalid. </exception><exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception><exception cref="T:System.NotSupportedException">The current Stream implementation does not support the write operation. </exception><filterpriority>2</filterpriority>
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public virtual IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state);

        /// <summary>
        /// Ends an asynchronous write operation.
        /// </summary>
        /// <param name="asyncResult">A reference to the outstanding asynchronous I/O request. </param><exception cref="T:System.ArgumentNullException"><paramref name="asyncResult"/> is null. </exception><exception cref="T:System.ArgumentException"><paramref name="asyncResult"/> did not originate from a <see cref="M:System.IO.Stream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)"/> method on the current stream. </exception><exception cref="T:System.IO.IOException">The stream is closed or an internal error has occurred.</exception><filterpriority>2</filterpriority>
        public virtual void EndWrite(IAsyncResult asyncResult);

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// 
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter. </param><param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position. </param><exception cref="T:System.IO.IOException">An I/O error occurs. </exception><exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output. </exception><exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception><filterpriority>1</filterpriority>
        public abstract long Seek(long offset, SeekOrigin origin);

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes. </param><exception cref="T:System.IO.IOException">An I/O error occurs. </exception><exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output. </exception><exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception><filterpriority>2</filterpriority>
        public abstract void SetLength(long value);

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// 
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source. </param><param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream. </param><param name="count">The maximum number of bytes to be read from the current stream. </param><exception cref="T:System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length. </exception><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative. </exception><exception cref="T:System.IO.IOException">An I/O error occurs. </exception><exception cref="T:System.NotSupportedException">The stream does not support reading. </exception><exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception><filterpriority>1</filterpriority>
        public abstract int Read([In, Out] byte[] buffer, int offset, int count);

        /// <summary>
        /// Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.
        /// </summary>
        /// 
        /// <returns>
        /// The unsigned byte cast to an Int32, or -1 if at the end of the stream.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception><exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception><filterpriority>2</filterpriority>
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public virtual int ReadByte();

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream. </param><param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream. </param><param name="count">The number of bytes to be written to the current stream. </param><exception cref="T:System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length. </exception><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative. </exception><exception cref="T:System.IO.IOException">An I/O error occurs. </exception><exception cref="T:System.NotSupportedException">The stream does not support writing. </exception><exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception><filterpriority>1</filterpriority>
        public abstract void Write(byte[] buffer, int offset, int count);

        /// <summary>
        /// Writes a byte to the current position in the stream and advances the position within the stream by one byte.
        /// </summary>
        /// <param name="value">The byte to write to the stream. </param><exception cref="T:System.IO.IOException">An I/O error occurs. </exception><exception cref="T:System.NotSupportedException">The stream does not support writing, or the stream is already closed. </exception><exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception><filterpriority>2</filterpriority>
        public virtual void WriteByte(byte value);

        /// <summary>
        /// Creates a thread-safe (synchronized) wrapper around the specified <see cref="T:System.IO.Stream"/> object.
        /// </summary>
        /// 
        /// <returns>
        /// A thread-safe <see cref="T:System.IO.Stream"/> object.
        /// </returns>
        /// <param name="stream">The <see cref="T:System.IO.Stream"/> object to synchronize.</param><exception cref="T:System.ArgumentNullException"><paramref name="stream"/> is null.</exception>
        [HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
        public static Stream Synchronized(Stream stream);

        /// <summary>
        /// Provides support for a <see cref="T:System.Diagnostics.Contracts.Contract"/>.
        /// </summary>
        protected virtual void ObjectInvariant();

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// 
        /// <returns>
        /// true if the stream supports reading; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public abstract bool CanRead { get; }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// 
        /// <returns>
        /// true if the stream supports seeking; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public abstract bool CanSeek { get; }

        /// <summary>
        /// Gets a value that determines whether the current stream can time out.
        /// </summary>
        /// 
        /// <returns>
        /// A value that determines whether the current stream can time out.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [ComVisible(false)]
        public virtual bool CanTimeout { get; }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// 
        /// <returns>
        /// true if the stream supports writing; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public abstract bool CanWrite { get; }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// 
        /// <returns>
        /// A long value representing the length of the stream in bytes.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking. </exception><exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception><filterpriority>1</filterpriority>
        public abstract long Length { get; }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// 
        /// <returns>
        /// The current position within the stream.
        /// </returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception><exception cref="T:System.NotSupportedException">The stream does not support seeking. </exception><exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception><filterpriority>1</filterpriority>
        public abstract long Position { get; set; }

        /// <summary>
        /// Gets or sets a value, in miliseconds, that determines how long the stream will attempt to read before timing out.
        /// </summary>
        /// 
        /// <returns>
        /// A value, in miliseconds, that determines how long the stream will attempt to read before timing out.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.IO.Stream.ReadTimeout"/> method always throws an <see cref="T:System.InvalidOperationException"/>. </exception><filterpriority>2</filterpriority>
        [ComVisible(false)]
        public virtual int ReadTimeout { get; set; }

        /// <summary>
        /// Gets or sets a value, in miliseconds, that determines how long the stream will attempt to write before timing out.
        /// </summary>
        /// 
        /// <returns>
        /// A value, in miliseconds, that determines how long the stream will attempt to write before timing out.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.IO.Stream.WriteTimeout"/> method always throws an <see cref="T:System.InvalidOperationException"/>. </exception><filterpriority>2</filterpriority>
        [ComVisible(false)]
        public virtual int WriteTimeout { get; set; }
    }
}
