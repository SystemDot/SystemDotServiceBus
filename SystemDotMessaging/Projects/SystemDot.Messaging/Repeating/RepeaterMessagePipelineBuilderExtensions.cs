using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Repeating
{
    static class RepeaterMessagePipelineBuilderExtensions
    {
        public static ProcessorBuilder<MessagePayload> ToSimpleMessageRepeater(
            this ProcessorBuilder<MessagePayload> builder,
            IMessageCache messageCache,
            ISystemTime systemTime,
            ITaskRepeater taskRepeater)
        {
            return builder.ToMessageRepeater(messageCache, systemTime, taskRepeater, new NullRepeatStrategy());
        }

        public static ProcessorBuilder<MessagePayload> ToMessageRepeater(
            this ProcessorBuilder<MessagePayload> builder, 
            IMessageCache messageCache, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater, 
            IRepeatStrategy strategy)
        {
            var repeater = new MessageRepeater(strategy, systemTime, messageCache);
            taskRepeater.Register(TimeSpan.FromSeconds(1), repeater.Repeat);

            return builder.ToProcessor(repeater);
        }
    }
}