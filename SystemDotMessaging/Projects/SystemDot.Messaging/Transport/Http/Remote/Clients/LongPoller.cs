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

namespace SystemDot.Messaging.Transport.Http.Remote.Clients
{
    class LongPoller
    {
        readonly IWebRequestor requestor;
        readonly ISerialiser formatter;
        readonly ITaskStarter starter;
        readonly IMessageReceiver receiver;

        public LongPoller(
            IWebRequestor requestor, 
            ISerialiser formatter, 
            ITaskStarter starter, 
            IMessageReceiver receiver)
        {
            Contract.Requires(requestor != null);
            Contract.Requires(formatter != null);
            Contract.Requires(starter != null);
            Contract.Requires(receiver != null);

            this.requestor = requestor;
            this.formatter = formatter;
            this.starter = starter;
            this.receiver = receiver;
        }

        public void ListenTo(ServerPath toListenFor)
        {
            Contract.Requires(toListenFor != null);

            StartNextPoll(toListenFor);
        }

        void Poll(ServerPath toListenFor)
        {
            Logger.Info("Long polling for messages for {0}", toListenFor);

            try
            {
                this.requestor.SendPut(
                    toListenFor.GetUrl(),
                    requestStream => this.formatter.Serialise(requestStream, CreateLongPollPayload(toListenFor)),
                    response =>
                    {
                        RecieveResponse(response);
                        StartNextPoll(toListenFor);
                    });
            }
            catch (Exception)
            {
                StartNextPoll(toListenFor);
            }
        }

        void StartNextPoll(ServerPath toListenFor)
        {
            this.starter.StartTask(() => Poll(toListenFor));
        }

        MessagePayload CreateLongPollPayload(ServerPath toListenFor)
        {
            var payload = new MessagePayload();
            payload.SetLongPollRequest(toListenFor);

            return payload;
        }

        void RecieveResponse(Stream responseStream)
        {
            var messages = this.formatter.Deserialise(responseStream).As<IEnumerable<MessagePayload>>();

            foreach (var message in messages)
            {
                Logger.Info("Recieved message");
                this.receiver.InputMessage(message);
            }
        }
    }
}