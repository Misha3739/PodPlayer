using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

using Foundation;

namespace ToDoList.HtppClient
{
    class DataTaskDelegate : NSUrlSessionDataDelegate
    {
        NativeMessageHandler This { get; set; }

        public DataTaskDelegate(NativeMessageHandler that)
        {
            this.This = that;
        }

        public override void DidReceiveResponse(NSUrlSession session, NSUrlSessionDataTask dataTask, NSUrlResponse response, Action<NSUrlSessionResponseDisposition> completionHandler)
        {
            var data = getResponseForTask(dataTask);

            try
            {
                if (data.CancellationToken.IsCancellationRequested)
                {
                    dataTask.Cancel();
                }

                var resp = (NSHttpUrlResponse)response;
                var req = data.Request;


                var content = new CancellableStreamContent(data.ResponseBody, () =>
                {
                    if (!data.IsCompleted)
                    {
                        dataTask.Cancel();
                    }
                    data.IsCompleted = true;

                    data.ResponseBody.SetException(new OperationCanceledException());
                });

                content.Progress = data.Progress;

                // NB: The double cast is because of a Xamarin compiler bug
                int status = (int)resp.StatusCode;
                var ret = new HttpResponseMessage((HttpStatusCode)status)
                {
                    Content = content,
                    RequestMessage = data.Request,
                };
                ret.RequestMessage.RequestUri = new Uri(resp.Url.AbsoluteString);

                foreach (var v in resp.AllHeaderFields)
                {
                    // NB: Cocoa trolling us so hard by giving us back dummy
                    // dictionary entries
                    if (v.Key == null || v.Value == null) continue;

                    ret.Headers.TryAddWithoutValidation(v.Key.ToString(), v.Value.ToString());
                    ret.Content.Headers.TryAddWithoutValidation(v.Key.ToString(), v.Value.ToString());
                }

                data.FutureResponse.TrySetResult(ret);
            }
            catch (Exception ex)
            {
                data.FutureResponse.TrySetException(ex);
            }

            completionHandler(NSUrlSessionResponseDisposition.Allow);
        }

        public override void WillCacheResponse(NSUrlSession session, NSUrlSessionDataTask dataTask,
            NSCachedUrlResponse proposedResponse, Action<NSCachedUrlResponse> completionHandler)
        {
            completionHandler(This.DisableCaching ? null : proposedResponse);
        }

        public override void DidCompleteWithError(NSUrlSession session, NSUrlSessionTask task, NSError error)
        {
            var data = getResponseForTask(task);
            data.IsCompleted = true;

            if (error != null)
            {
                var ex = new WebException(error.Description);

                // Pass the exception to the response
                data.FutureResponse.TrySetException(ex);
                data.ResponseBody.SetException(ex);
                return;
            }

            data.ResponseBody.Complete();

            lock (This.inflightRequests)
            {
                This.inflightRequests.Remove(task);
            }
        }

        public override void DidReceiveData(NSUrlSession session, NSUrlSessionDataTask dataTask, NSData byteData)
        {
            var data = getResponseForTask(dataTask);
            var bytes = byteData.ToArray();

            // NB: If we're cancelled, we still might have one more chunk 
            // of data that attempts to be delivered
            if (data.IsCompleted) return;

            data.ResponseBody.AddByteArray(bytes);
        }

        InflightOperation getResponseForTask(NSUrlSessionTask task)
        {
            lock (This.inflightRequests)
            {
                return This.inflightRequests[task];
            }
        }

        static readonly Regex cnRegex = new Regex(@"CN\s*=\s*([^,]*)", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        public override void WillPerformHttpRedirection(NSUrlSession session, NSUrlSessionTask task, NSHttpUrlResponse response, NSUrlRequest newRequest, Action<NSUrlRequest> completionHandler)
        {
            NSUrlRequest nextRequest = (This.AllowAutoRedirect ? newRequest : null);
            completionHandler(nextRequest);
        }


    }
}
