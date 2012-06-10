using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Messaging.Pipes;

namespace SystemDot.Messaging.Sending
{
    public class MessageSender 
    {
        readonly string address;
        readonly BinaryFormatter formatter;

        public MessageSender(IPipe pipe, string address)
        {
            Contract.Requires(pipe != null);
            Contract.Requires(!string.IsNullOrEmpty(address));

            this.address = address;
            pipe.MessagePublished += OnMessagePublishedToPipe;
            this.formatter = new BinaryFormatter();
        }

        private void OnMessagePublishedToPipe(object message)
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
            catch(WebException)
            {
            }
        }
    }
}