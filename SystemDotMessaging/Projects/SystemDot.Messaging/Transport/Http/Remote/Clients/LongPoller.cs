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
        readonly ITaskScheduler scheduler;
        readonly IMessageReceiver receiver;
        readonly ITaskStarter starter;

        public LongPoller(
            IWebRequestor requestor, 
            ISerialiser formatter, 
            ITaskScheduler scheduler, 
            IMessageReceiver receiver, 
            ITaskStarter starter)
        {
            Contract.Requires(requestor != null);
            Contract.Requires(formatter != null);
            Contract.Requires(scheduler != null);
            Contract.Requires(receiver != null);
            Contract.Requires(starter != null);

            this.requestor = requestor;
            this.formatter = formatter;
            this.scheduler = scheduler;
            this.receiver = receiver;
            this.starter = starter;
        }

        public void ListenTo(ServerRoute toListenFor)
        {
            Contract.Requires(toListenFor != null);

            StartNextPoll(toListenFor);
        }

        void Poll(ServerRoute toListenFor)
        {
            Logger.Info("Long polling for messages for {0}", toListenFor);

            this.requestor.SendPut(
                toListenFor.GetUrl(),
                requestStream => this.formatter.Serialise(requestStream, CreateLongPollPayload(toListenFor)),
                RecieveResponse,
                e =>
                {
                    Logger.Info("Cannot long poll server: {0}", e.Message);
                    StartNextPoll(toListenFor, TimeSpan.FromSeconds(4));
                },
                () => StartNextPoll(toListenFor));
        }

        void StartNextPoll(ServerRoute toListenFor)
        {
            this.starter.StartTask(() => Poll(toListenFor));
        }

        void StartNextPoll(ServerRoute toListenFor, TimeSpan after)
        {
            this.scheduler.ScheduleTask(after,() => Poll(toListenFor));
        }

        MessagePayload CreateLongPollPayload(ServerRoute toListenFor)
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