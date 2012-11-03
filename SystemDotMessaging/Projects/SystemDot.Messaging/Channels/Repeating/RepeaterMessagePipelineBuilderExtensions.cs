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
            IPersistence persistence,
            ICurrentDateProvider currentDateProvider,
            ITaskRepeater taskRepeater)
        {
            return builder.ToMessageRepeater(
                currentDateProvider, 
                taskRepeater, 
                new EscalatingTimeRepeatStrategy(currentDateProvider, persistence));
        }


        public static ProcessorBuilder<MessagePayload> ToSimpleMessageRepeater(
            this ProcessorBuilder<MessagePayload> builder,
            IPersistence persistence,
            ICurrentDateProvider currentDateProvider,
            ITaskRepeater taskRepeater)
        {
            return builder.ToMessageRepeater(
                currentDateProvider,
                taskRepeater,
                new SimpleRepeatStrategy(persistence));
        }

        static ProcessorBuilder<MessagePayload> ToMessageRepeater(
            this ProcessorBuilder<MessagePayload> builder,
            ICurrentDateProvider currentDateProvider,
            ITaskRepeater taskRepeater,
            IRepeatStrategy strategy)
        {
            var repeater = new MessageRepeater(strategy, currentDateProvider);
            taskRepeater.Register(TimeSpan.FromSeconds(1), repeater.Start);

            return builder.ToProcessor(repeater);
        }
    }
}