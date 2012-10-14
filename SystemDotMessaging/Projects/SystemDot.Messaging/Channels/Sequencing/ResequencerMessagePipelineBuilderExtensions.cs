using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.Sequencing
{
    public static class ResequencerMessagePipelineBuilderExtensions
    {
        public static ProcessorBuilder<MessagePayload> ToResequencerIfSequenced(
            this ProcessorBuilder<MessagePayload> builder,
            IMessageSender sender,
            IPersistence persistence,
            RecieveChannelSchema schema)
        {
            IMessageProcessor<MessagePayload, MessagePayload> processor;
            
            if(schema.IsSequenced) 
                processor = new Resequencer(persistence);    
            else 
                processor = new MessageAcknowledger(sender);

            return builder.ToProcessor(processor);
        }
    }
}