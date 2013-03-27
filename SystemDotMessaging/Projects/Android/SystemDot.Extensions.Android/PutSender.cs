using System.IO;
using System.Net;

namespace System
{
    public static class PutSender
    {
        public static void Send(Action<Stream> toPerformOnRequest, Action<Stream> toPerformOnResponse, string url)
        {
            var request = CreateRequest(url);

            SendRequest(toPerformOnRequest, request);
            RecieveResponse(toPerformOnResponse, request);
        }

        static HttpWebRequest CreateRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
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