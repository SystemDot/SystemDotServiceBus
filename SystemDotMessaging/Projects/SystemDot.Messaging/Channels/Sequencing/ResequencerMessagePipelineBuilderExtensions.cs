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
            IPersistence persistence,
            ChannelSchema schema)
        {
            IMessageProcessor<MessagePayload, MessagePayload> processor;
            
            if(schema.IsDurable)
                processor = new Resequencer(persistence);    
            else 
                processor = new MessageDecacher(persistence);

            return builder.ToProcessor(processor);
        }
    }
}