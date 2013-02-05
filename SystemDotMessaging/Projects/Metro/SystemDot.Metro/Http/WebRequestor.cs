using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SystemDot.Logging;

namespace SystemDot.Http
{
    public class WebRequestor : IWebRequestor
    {
        public void SendPut(FixedPortAddress address, Action<Stream> toPerformOnRequest)
        {
            SendPut(address, toPerformOnRequest, s => { });
        }

        public async void SendPut(
            FixedPortAddress address,
            Action<Stream> toPerformOnRequest,
            Action<Stream> toPerformOnResponse)
        {
            try
            {
                ProcessResponse(toPerformOnResponse, await SendRequest(address, toPerformOnRequest));
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        static async Task<HttpResponseMessage> SendRequest(FixedPortAddress address, Action<Stream> toPerformOnRequest)
        {
            var stream = new MemoryStream();
            toPerformOnRequest(stream);
            stream.Position = 0;

            HttpResponseMessage response = await new HttpClient()
                .SendAsync(
                    new HttpRequestMessage(HttpMethod.Put, address.Url)
                    {
                        Content = new StreamContent(stream)
                    });

            stream.Dispose();

            return response;
        }

        static async void ProcessResponse(Action<Stream> toPerformOnResponse, HttpResponseMessage response)
        {
            var responseStream = await response.Content.ReadAsStreamAsync();
            responseStream.Position = 0;
            toPerformOnResponse(responseStream);

            responseStream.Dispose();
        }
    }
}