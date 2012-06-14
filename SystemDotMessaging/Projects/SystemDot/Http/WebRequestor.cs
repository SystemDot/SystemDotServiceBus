using System;
using System.IO;
using System.Net;
using SystemDot.Logging;

namespace SystemDot.Http
{
    public class WebRequestor : IWebRequestor
    {
        public void SendPut(string address, Action<Stream> performOnRequestStream)
        {
            SendPut(address, performOnRequestStream, s => { });
        }

        public void SendPut(string address, Action<Stream> performOnRequestStream, Action<Stream> performOnResponseStream)
        {
            var request = (HttpWebRequest)WebRequest.Create(address);
            request.Method = "PUT";
            request.ConnectionGroupName = Guid.NewGuid().ToString();

            try
            {
                using (var stream = request.GetRequestStream()) 
                    performOnRequestStream(stream);

                using (var stream = request.GetResponse().GetResponseStream()) 
                    performOnResponseStream(stream);
            }
            catch (WebException e)
            {
                Logger.Log(e.Message);
            }
        }
    }
}