using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Sequencing
{
    static class ResequencerMessagePipelineBuilderExtensions
    {
        public static ProcessorBuilder<MessagePayload> ToResequencerIfSequenced(
            this ProcessorBuilder<MessagePayload> builder,
            ReceiveMessageCache messageCache,
            ISequenceOptionSchema schema)
        {
            IMessageProcessor<MessagePayload, MessagePayload> processor;
            
            if(schema.IsSequenced)
                processor = new Resequencer(messageCache);    
            else 
                processor = new MessageDecacher(messageCache);

            return builder.ToProcessor(processor);
        }
    }
}