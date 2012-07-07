using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

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
            request.ConnectionGroupName = Guid.NewGuid().ToString();

            SendRequest(toPerformOnRequest, request);
            return RecieveResponse(toPerformOnResponse, request);   
        }

        private static void SendRequest(Action<Stream> toPerformOnRequest, HttpWebRequest request)
        {
            Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, request)
                .ContinueWith(task => PerformActionOnStream(toPerformOnRequest, task.Result))
                .Wait();
        }

        private static Task RecieveResponse(Action<Stream> toPerformOnResponse, HttpWebRequest request)
        {
            return Task.Factory
                .FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, request)
                .ContinueWith(task => PerformActionOnStream(toPerformOnResponse, task.Result.GetResponseStream()));
        }

        private static void PerformActionOnStream(Action<Stream> toPerform, Stream stream)
        {
            using (stream) toPerform(stream);
        }
    }
}