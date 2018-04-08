using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Foundation;
using Security;

namespace ToDoList.HtppClient
{
    public delegate void ProgressDelegate(long bytes, long totalBytes, long totalBytesExpected);
    public class NativeMessageHandler : HttpClientHandler
    {
        readonly NSUrlSession session;

        internal readonly Dictionary<NSUrlSessionTask, InflightOperation> inflightRequests =
            new Dictionary<NSUrlSessionTask, InflightOperation>();

        readonly Dictionary<HttpRequestMessage, ProgressDelegate> registeredProgressCallbacks =
            new Dictionary<HttpRequestMessage, ProgressDelegate>();

        readonly Dictionary<string, string> headerSeparators =
            new Dictionary<string, string>(){
                {"User-Agent", " "}
            };

        internal readonly bool throwOnCaptiveNetwork;
        internal readonly bool customSSLVerification;

        public bool DisableCaching { get; set; }

        public NativeMessageHandler() : this(false) { }
        public NativeMessageHandler(bool customSSLVerification, SslProtocol? minimumSSLProtocol = null)
        {
            var configuration = NSUrlSessionConfiguration.DefaultSessionConfiguration;

            // System.Net.ServicePointManager.SecurityProtocol provides a mechanism for specifying supported protocol types
            // for System.Net. Since iOS only provides an API for a minimum and maximum protocol we are not able to port
            // this configuration directly and instead use the specified minimum value when one is specified.
            if (minimumSSLProtocol.HasValue)
            {
                configuration.TLSMinimumSupportedProtocol = minimumSSLProtocol.Value;
            }

            session = NSUrlSession.FromConfiguration(
                NSUrlSessionConfiguration.DefaultSessionConfiguration,
                (INSUrlSessionDataDelegate)new DataTaskDelegate(this), null);

            this.customSSLVerification = customSSLVerification;

            this.DisableCaching = false;
        }

        string getHeaderSeparator(string name)
        {
            if (headerSeparators.ContainsKey(name))
            {
                return headerSeparators[name];
            }

            return ",";
        }

        public void RegisterForProgress(HttpRequestMessage request, ProgressDelegate callback)
        {
            if (callback == null && registeredProgressCallbacks.ContainsKey(request))
            {
                registeredProgressCallbacks.Remove(request);
                return;
            }

            registeredProgressCallbacks[request] = callback;
        }

        ProgressDelegate getAndRemoveCallbackFromRegister(HttpRequestMessage request)
        {
            ProgressDelegate emptyDelegate = delegate { };

            lock (registeredProgressCallbacks)
            {
                if (!registeredProgressCallbacks.ContainsKey(request)) return emptyDelegate;

                var callback = registeredProgressCallbacks[request];
                registeredProgressCallbacks.Remove(request);
                return callback;
            }
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var headers = request.Headers as IEnumerable<KeyValuePair<string, IEnumerable<string>>>;
            var ms = new MemoryStream();

            if (request.Content != null)
            {
                await request.Content.CopyToAsync(ms).ConfigureAwait(false);
                headers = headers.Union(request.Content.Headers).ToArray();
            }

            var rq = new NSMutableUrlRequest()
            {
                AllowsCellularAccess = true,
                Body = NSData.FromArray(ms.ToArray()),
                CachePolicy = (!this.DisableCaching ? NSUrlRequestCachePolicy.UseProtocolCachePolicy : NSUrlRequestCachePolicy.ReloadIgnoringCacheData),
                Headers = headers.Aggregate(new NSMutableDictionary(), (acc, x) =>
                {
                    acc.Add(new NSString(x.Key), new NSString(String.Join(getHeaderSeparator(x.Key), x.Value)));
                    return acc;
                }),
                HttpMethod = request.Method.ToString().ToUpperInvariant(),
                Url = NSUrl.FromString(request.RequestUri.AbsoluteUri),
            };

            var op = session.CreateDataTask(rq);

            cancellationToken.ThrowIfCancellationRequested();

            var ret = new TaskCompletionSource<HttpResponseMessage>();
            cancellationToken.Register(() => ret.TrySetCanceled());

            lock (inflightRequests)
            {
                inflightRequests[op] = new InflightOperation()
                {
                    FutureResponse = ret,
                    Request = request,
                    Progress = getAndRemoveCallbackFromRegister(request),
                    ResponseBody = new ByteArrayListStream(),
                    CancellationToken = cancellationToken,
                };
            }

            op.Resume();
            return await ret.Task.ConfigureAwait(false);
        }
    }
}
