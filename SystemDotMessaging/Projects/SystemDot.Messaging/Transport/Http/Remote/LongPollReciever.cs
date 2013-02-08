using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using SystemDot.Http;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.Http.Remote
{
    public class LongPollReciever : IMessageReciever
    {
        readonly IWebRequestor requestor;
        readonly ISerialiser formatter;
        readonly ITaskStarter starter;

        public event Action<MessagePayload> MessageProcessed;

        public LongPollReciever(IWebRequestor requestor, ISerialiser formatter, ITaskStarter starter)
        {
            Contract.Requires(requestor != null);
            Contract.Requires(formatter != null);
            Contract.Requires(starter != null);

            this.requestor = requestor;
            this.formatter = formatter;
            this.starter = starter;
        }

        public void RegisterAddress(EndpointAddress address)
        {
            Contract.Requires(address != EndpointAddress.Empty);

            StartNextPoll(address);
        }

        void Poll(EndpointAddress address)
        {
            Logger.Info("Long polling for messages for {0}", address);

            try
            {
                this.requestor.SendPut(
                    address.GetUrl(),
                    requestStream => this.formatter.Serialise(requestStream, CreateLongPollPayload(address)),
                    response =>
                    {
                        RecieveResponse(response);
                        StartNextPoll(address);
                    });
            }
            catch (Exception)
            {
                StartNextPoll(address);
            }
        }

        void StartNextPoll(EndpointAddress address)
        {
            this.starter.StartTask(() => Poll(address));
        }

        MessagePayload CreateLongPollPayload(EndpointAddress address)
        {
            var payload = new MessagePayload();
            payload.SetLongPollRequest(address);

            return payload;
        }

        void RecieveResponse(Stream responseStream)
        {
            var messages = this.formatter.Deserialise(responseStream).As<IEnumerable<MessagePayload>>();

            foreach (var message in messages)
            {
                Logger.Info("Recieved message");
                this.MessageProcessed(message);
            }
        }
    }
}