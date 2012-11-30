using SystemDot.Messaging.Channels.Builders;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Sequencing
{
    public static class ResequencerMessagePipelineBuilderExtensions
    {
        public static ProcessorBuilder<MessagePayload> ToResequencerIfSequenced(
            this ProcessorBuilder<MessagePayload> builder,
            MessageCache messageCache,
            ChannelSchema schema)
        {
            IMessageProcessor<MessagePayload, MessagePayload> processor;
            
            if(schema.IsDurable)
                processor = new Resequencer(messageCache);    
            else 
                processor = new MessageDecacher(messageCache);

            return builder.ToProcessor(processor);
        }
    }
}