using System;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Channels.Repeating
{
    public static class RepeaterMessagePipelineBuilderExtensions
    {
        public static ProcessorBuilder<MessagePayload> ToMessageRepeater(
            this ProcessorBuilder<MessagePayload> builder,
            IMessageCache cache,
            ICurrentDateProvider currentDateProvider,
            ITaskRepeater taskRepeater)
        {
            var messageRepeater = new MessageRepeater(cache, currentDateProvider);
            taskRepeater.Register(TimeSpan.FromSeconds(1), messageRepeater.Start);

            return builder.ToProcessor(messageRepeater);
        }
    }
}