using System;
using System.IO;
using System.Net;
using SystemDot.Logging;

namespace SystemDot.Http
{
    public class WebRequestor : IWebRequestor
    {
        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest)
        {
            SendPut(address, toPerformOnRequest, s => { });
        }

        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest, Action<Stream> toPerformOnResponse)
        {
            try
            {
                var request = CreateRequest(address);

                SendRequest(toPerformOnRequest, request);
                RecieveResponse(toPerformOnResponse, request);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        static HttpWebRequest CreateRequest(FixedPortAddress address)
        {
            var request = (HttpWebRequest) WebRequest.Create(address.Url);
            request.Method = "PUT";
            return request;
        }

        static void SendRequest(Action<Stream> toPerformOnRequest, HttpWebRequest request)
        {
            using (var stream = request.GetRequestStream())
                toPerformOnRequest(stream);
        }

        static void RecieveResponse(Action<Stream> toPerformOnResponse, HttpWebRequest request)
        {
            using (var response = request.GetResponse())
            using (Stream stream = response.GetResponseStream())
                toPerformOnResponse(stream);
        }
    }
}