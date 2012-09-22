using System;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing.Caching;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Messages.Processing.Repeating
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