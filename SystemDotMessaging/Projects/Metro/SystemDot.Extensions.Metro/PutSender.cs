using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SystemDot
{
    public class PutSender
    {
        public static async void Send(Action<Stream> toPerformOnRequest, Action<Stream> toPerformOnResponse, string url)
        {
              ProcessResponse(toPerformOnResponse, await SendRequest(toPerformOnRequest, url));
        }

        static async Task<HttpResponseMessage> SendRequest(Action<Stream> toPerformOnRequest, string url)
        {
            var stream = new MemoryStream();
            toPerformOnRequest(stream);
            stream.Position = 0;

            HttpResponseMessage response = await new HttpClient()
                .SendAsync(
                    new HttpRequestMessage(HttpMethod.Put, url)
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