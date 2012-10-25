using System;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Channels.RequestReply.Repeating
{
    public static class RepeaterMessagePipelineBuilderExtensions
    {
        public static ProcessorBuilder<MessagePayload> ToMessageRepeater(
            this ProcessorBuilder<MessagePayload> builder,
            IPersistence persistence,
            ICurrentDateProvider currentDateProvider,
            ITaskRepeater taskRepeater)
        {
            var messageRepeater = new MessageRepeater(persistence, currentDateProvider);
            taskRepeater.Register(TimeSpan.FromSeconds(1), messageRepeater.Start);

            return builder.ToProcessor(messageRepeater);
        }
    }
}