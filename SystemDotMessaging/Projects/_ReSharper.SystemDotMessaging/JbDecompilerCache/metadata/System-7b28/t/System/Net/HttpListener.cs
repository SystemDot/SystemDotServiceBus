// Type: System.Net.HttpListener
// Assembly: System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.dll

using System;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Permissions;

namespace System.Net
{
    public sealed class HttpListener : IDisposable
    {
        #region Delegates

        public delegate ExtendedProtectionPolicy ExtendedProtectionSelector(HttpListenerRequest request);

        #endregion

        public HttpListener();
        public AuthenticationSchemeSelector AuthenticationSchemeSelectorDelegate { get; set; }
        public HttpListener.ExtendedProtectionSelector ExtendedProtectionSelectorDelegate { get; set; }
        public AuthenticationSchemes AuthenticationSchemes { get; set; }
        public ExtendedProtectionPolicy ExtendedProtectionPolicy { get; set; }
        public ServiceNameCollection DefaultServiceNames { get; }
        public string Realm { get; set; }
        public static bool IsSupported { get; }
        public bool IsListening { get; }
        public bool IgnoreWriteExceptions { get; set; }
        public bool UnsafeConnectionNtlmAuthentication { get; set; }
        public HttpListenerPrefixCollection Prefixes { get; }

        #region IDisposable Members

        void IDisposable.Dispose();

        #endregion

        public void Start();
        public void Stop();
        public void Abort();
        public void Close();
        public HttpListenerContext GetContext();

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginGetContext(AsyncCallback callback, object state);

        public HttpListenerContext EndGetContext(IAsyncResult asyncResult);
    }
}
