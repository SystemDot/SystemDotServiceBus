using System;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Channels.Repeating
{
    public static class RepeaterMessagePipelineBuilderExtensions
    {
        public static ProcessorBuilder<MessagePayload> ToEscalatingTimeMessageRepeater(
            this ProcessorBuilder<MessagePayload> builder,
            MessageCache messageCache,
            ICurrentDateProvider currentDateProvider,
            ITaskRepeater taskRepeater)
        {
            return builder.ToMessageRepeater(messageCache, currentDateProvider, taskRepeater, new EscalatingTimeRepeatStrategy());
        }


        public static ProcessorBuilder<MessagePayload> ToSimpleMessageRepeater(
            this ProcessorBuilder<MessagePayload> builder,
            MessageCache messageCache,
            ICurrentDateProvider currentDateProvider,
            ITaskRepeater taskRepeater)
        {
            return builder.ToMessageRepeater(messageCache, currentDateProvider, taskRepeater, new SimpleRepeatStrategy());
        }

        public static ProcessorBuilder<MessagePayload> ToMessageRepeater(
            this ProcessorBuilder<MessagePayload> builder, 
            MessageCache messageCache, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater, 
            IRepeatStrategy strategy)
        {
            var repeater = new MessageRepeater(strategy, currentDateProvider, messageCache);
            taskRepeater.Register(TimeSpan.FromSeconds(1), repeater.Start);

            return builder.ToProcessor(repeater);
        }
    }
}