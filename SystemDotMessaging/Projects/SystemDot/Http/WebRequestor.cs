using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using SystemDot.Logging;

namespace SystemDot.Http
{
    public class WebRequestor : IWebRequestor
    {
        public Task SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest)
        {
            return SendPut(address, toPerformOnRequest, s => { });
        }

        public Task SendPut(
            FixedPortAddress address,
            Action<Stream> toPerformOnRequest,
            Action<Stream> toPerformOnResponse)
        {
            var request = (HttpWebRequest) WebRequest.Create(address.Url);
            request.Method = "PUT";
            
            SendRequest(toPerformOnRequest, request);
            return RecieveResponse(toPerformOnResponse, request);   
        }

        private static void SendRequest(Action<Stream> toPerformOnRequest, HttpWebRequest request)
        {
            var requestTask = Task.Factory.FromAsync<Stream>(
                request.BeginGetRequestStream, 
                request.EndGetRequestStream,
                request)
                .ContinueWith(task => PerformActionOnRequest(toPerformOnRequest, task));

            requestTask.Wait();
        }

        static void PerformActionOnRequest(Action<Stream> toPerformOnRequest, Task<Stream> task)
        {
            try
            {
                using (task.Result) 
                    toPerformOnRequest(task.Result);
            }
            catch (AggregateException e)
            {
                LogAggregateException(e);
            }
        }

        private static Task RecieveResponse(Action<Stream> toPerformOnResponse, HttpWebRequest request)
        {
            return Task.Factory
                .FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, request)
                .ContinueWith(task => PerformActionOnResponse(toPerformOnResponse, task));
        }

        static void PerformActionOnResponse(Action<Stream> toPerformOnResponse, Task<WebResponse> task)
        {
            try
            {
                using (task.Result.GetResponseStream()) 
                    toPerformOnResponse(task.Result.GetResponseStream());
            }
            catch (AggregateException e)
            {
                LogAggregateException(e);
            }
        }

        private static void LogAggregateException(AggregateException toLog)
        {
            toLog.Handle(e =>
            {
                Logger.Error(e.Message);
                return true;
            });
        }
    }
}