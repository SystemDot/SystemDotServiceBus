using System;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace SystemDot.Messaging.Channels.Sending.Http
{
    public class MessageWebRequestor : IMessageWebRequestor
    {
        readonly string address;
        readonly BinaryFormatter formatter;

        public MessageWebRequestor(string address)
        {
            this.address = address;
            this.formatter = new BinaryFormatter();
        }

        public void PutMessage(object message)
        {
            var request = (HttpWebRequest)WebRequest.Create(address);
            request.Method = "PUT";
            request.ConnectionGroupName = Guid.NewGuid().ToString();

            try
            {
                using (var stream = request.GetRequestStream())
                    formatter.Serialize(stream, message);

                request.GetResponse().Close();
            }
            catch(WebException) { }
        }
    }
}