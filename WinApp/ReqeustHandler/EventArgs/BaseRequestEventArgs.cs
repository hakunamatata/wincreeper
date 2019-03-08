using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.ReqeustHandler
{
    public abstract class BaseRequestEventArgs : System.EventArgs
    {
        protected BaseRequestEventArgs(IWebBrowser browserControl, IBrowser browser)
        {
            BrowserControl = browserControl;
            Browser = browser;
        }

        public IWebBrowser BrowserControl { get; private set; }
        public IBrowser Browser { get; private set; }
    }

    public class CanGetCookiesEventArg : BaseRequestEventArgs
    {
        public CanGetCookiesEventArg(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request)
            : base(browserControl, browser)
        {
            Frame = frame;
            Request = request;
        }

        public IFrame Frame { get; private set; }
        public IRequest Request { get; private set; }
        public bool IsRedirect { get; private set; }

        /// <summary>
        /// Return true to allow cookies to be included in the network request or false to block cookies.
        /// </summary>
        public bool GetCookies { get; set; } = true;
    }

    public class CanSetCookieEventArg : BaseRequestEventArgs
    {
        public CanSetCookieEventArg(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, Cookie cookie)
            : base(browserControl, browser)
        {
            Frame = frame;
            Request = request;
            Cookie = cookie;
        }

        public IFrame Frame { get; private set; }
        public IRequest Request { get; private set; }
        public bool IsRedirect { get; private set; }
        public Cookie Cookie { get; private set; }

        /// <summary>
        /// Return true to allow the cookie to be stored or false to block the cookie.
        /// </summary>
        public bool SetCookie { get; set; } = true;
    }

    public class GetAuthCredentialsEventArgs : BaseRequestEventArgs
    {
        public GetAuthCredentialsEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback) : base(browserControl, browser)
        {
            Frame = frame;
            IsProxy = isProxy;
            Host = host;
            Port = port;
            Realm = realm;
            Scheme = scheme;
            Callback = callback;

            ContinueAsync = false; // default
        }
        public IFrame Frame { get; private set; }
        public bool IsProxy { get; private set; }
        public string Host { get; private set; }
        public int Port { get; private set; }
        public string Realm { get; private set; }
        public string Scheme { get; private set; }
        /// <summary>
        ///     Callback interface used for asynchronous continuation of authentication requests.
        /// </summary>
        public IAuthCallback Callback { get; private set; }

        /// <summary>
        ///     Set to true to continue the request and call
        ///     <see cref="T:CefSharp.GetAuthCredentialsEventArgs.Continue(System.String, System.String)" /> when the authentication information
        ///     is available. Set to false to cancel the request.
        /// </summary>
        public bool ContinueAsync { get; set; }
    }

    public class GetResourceResponseFilterEventArgs : BaseRequestEventArgs
    {
        public GetResourceResponseFilterEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response) : base(browserControl, browser)
        {
            Frame = frame;
            Request = request;
            Response = response;

            ResponseFilter = null; // default
        }

        public IFrame Frame { get; private set; }
        public IRequest Request { get; private set; }
        public IResponse Response { get; private set; }

        /// <summary>
        ///     Set a ResponseFilter to intercept this response, leave it null otherwise.
        /// </summary>
        public IResponseFilter ResponseFilter { get; set; }
    }

    public class OnBeforeBrowseEventArgs : BaseRequestEventArgs
    {
        public OnBeforeBrowseEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
            : base(browserControl, browser)
        {
            Frame = frame;
            Request = request;
            IsRedirect = isRedirect;
            UserGesture = userGesture;

            CancelNavigation = false; // default
        }

        public IFrame Frame { get; private set; }
        public IRequest Request { get; private set; }
        public bool IsRedirect { get; private set; }
        public bool UserGesture { get; private set; }

        /// <summary>
        ///     Set to true to cancel the navigation or false to allow the navigation to proceed.
        /// </summary>
        public bool CancelNavigation { get; set; }
    }
    public class OnBeforeResourceLoadEventArgs : BaseRequestEventArgs
    {
        public OnBeforeResourceLoadEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
            : base(browserControl, browser)
        {
            Frame = frame;
            Request = request;
            Callback = callback;

            ContinuationHandling = CefReturnValue.Continue; // default
        }

        public IFrame Frame { get; private set; }
        public IRequest Request { get; private set; }

        /// <summary>
        ///     Callback interface used for asynchronous continuation of url requests.
        /// </summary>
        public IRequestCallback Callback { get; private set; }

        /// <summary>
        ///     To cancel loading of the resource return <see cref="F:CefSharp.CefReturnValue.Cancel" />
        ///     or <see cref="F:CefSharp.CefReturnValue.Continue" /> to allow the resource to load normally. For async
        ///     return <see cref="F:CefSharp.CefReturnValue.ContinueAsync" /> and use
        ///     <see cref="OnBeforeResourceLoadEventArgs.Callback" /> to handle continuation.
        /// </summary>
        public CefReturnValue ContinuationHandling { get; set; }
    }
    public class OnCertificateErrorEventArgs : BaseRequestEventArgs
    {
        public OnCertificateErrorEventArgs(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
            : base(browserControl, browser)
        {
            ErrorCode = errorCode;
            RequestUrl = requestUrl;
            SSLInfo = sslInfo;
            Callback = callback;

            ContinueAsync = false; // default
        }

        public CefErrorCode ErrorCode { get; private set; }
        public string RequestUrl { get; private set; }
        public ISslInfo SSLInfo { get; private set; }

        /// <summary>
        ///     Callback interface used for asynchronous continuation of url requests.
        ///     If empty the error cannot be recovered from and the request will be canceled automatically.
        /// </summary>
        public IRequestCallback Callback { get; private set; }

        /// <summary>
        ///     Set to false to cancel the request immediately. Set to true and use <see cref="T:CefSharp.IRequestCallback" /> to
        ///     execute in an async fashion.
        /// </summary>
        public bool ContinueAsync { get; set; }
    }
    public class OnOpenUrlFromTabEventArgs : BaseRequestEventArgs
    {
        public OnOpenUrlFromTabEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
            : base(browserControl, browser)
        {
            Frame = frame;
            TargetUrl = targetUrl;
            TargetDisposition = targetDisposition;
            UserGesture = userGesture;

            CancelNavigation = false; // default
        }

        public IFrame Frame { get; private set; }
        public string TargetUrl { get; private set; }
        public WindowOpenDisposition TargetDisposition { get; private set; }
        public bool UserGesture { get; private set; }

        /// <summary>
        ///     Set to true to cancel the navigation or false to allow the navigation to proceed.
        /// </summary>
        public bool CancelNavigation { get; set; }
    }
    public class OnPluginCrashedEventArgs : BaseRequestEventArgs
    {
        public OnPluginCrashedEventArgs(IWebBrowser browserControl, IBrowser browser, string pluginPath) : base(browserControl, browser)
        {
            PluginPath = pluginPath;
        }

        public string PluginPath { get; private set; }
    }
    public class OnProtocolExecutionEventArgs : BaseRequestEventArgs
    {
        public OnProtocolExecutionEventArgs(IWebBrowser browserControl, IBrowser browser, string url) : base(browserControl, browser)
        {
            Url = url;

            AttemptExecution = false; // default
        }

        public string Url { get; private set; }

        /// <summary>
        ///     Set to true to attempt execution via the registered OS protocol handler, if any. Otherwise set to false.
        /// </summary>
        public bool AttemptExecution { get; set; }
    }
    public class OnQuotaRequestEventArgs : BaseRequestEventArgs
    {
        public OnQuotaRequestEventArgs(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
            : base(browserControl, browser)
        {
            OriginUrl = originUrl;
            NewSize = newSize;
            Callback = callback;

            ContinueAsync = false; // default
        }

        public string OriginUrl { get; private set; }
        public long NewSize { get; private set; }

        /// <summary>
        ///     Callback interface used for asynchronous continuation of url requests.
        /// </summary>
        public IRequestCallback Callback { get; private set; }

        /// <summary>
        ///     Set to false to cancel the request immediately. Set to true to continue the request
        ///     and call <see cref="T:OnQuotaRequestEventArgs.Callback.Continue(System.Boolean)" /> either in this method or at a later
        ///     time to grant or deny the request.
        /// </summary>
        public bool ContinueAsync { get; set; }
    }
    public class OnRenderProcessTerminatedEventArgs : BaseRequestEventArgs
    {
        public OnRenderProcessTerminatedEventArgs(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
            : base(browserControl, browser)
        {
            Status = status;
        }

        public CefTerminationStatus Status { get; private set; }
    }
    public class OnRenderViewReadyEventArgs : BaseRequestEventArgs
    {
        public OnRenderViewReadyEventArgs(IWebBrowser browserControl, IBrowser browser) : base(browserControl, browser)
        {
        }
    }
    public class OnResourceLoadCompleteEventArgs : BaseRequestEventArgs
    {
        public OnResourceLoadCompleteEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
            : base(browserControl, browser)
        {
            Frame = frame;
            Request = request;
            Response = response;
            Status = status;
            ReceivedContentLength = receivedContentLength;
        }

        public IFrame Frame { get; private set; }
        public IRequest Request { get; private set; }
        public IResponse Response { get; private set; }
        public UrlRequestStatus Status { get; private set; }
        public long ReceivedContentLength { get; private set; }
    }

    public class OnResourceRedirectEventArgs : BaseRequestEventArgs
    {
        public OnResourceRedirectEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, string newUrl)
            : base(browserControl, browser)
        {
            Frame = frame;
            Request = request;
            Response = response;
            NewUrl = newUrl;
        }

        public IFrame Frame { get; private set; }
        public IRequest Request { get; private set; }
        public IResponse Response { get; private set; }

        /// <summary>
        ///     the new URL and can be changed if desired.
        /// </summary>
        public string NewUrl { get; set; }
    }

    public class OnResourceResponseEventArgs : BaseRequestEventArgs
    {
        public OnResourceResponseEventArgs(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
            : base(browserControl, browser)
        {
            Frame = frame;
            Request = request;
            Response = response;

            RedirectOrRetry = false; // default
        }

        public IFrame Frame { get; private set; }
        public IRequest Request { get; private set; }
        public IResponse Response { get; private set; }

        /// <summary>
        ///     To allow the resource to load normally set to false.
        ///     To redirect or retry the resource, modify <see cref="OnBeforeResourceLoadEventArgs.Request" /> (url, headers or
        ///     post body) and set to true.
        /// </summary>
        public bool RedirectOrRetry { get; set; }
    }
}