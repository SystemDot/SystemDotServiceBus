using SystemDot.Messaging.Channels.Messages.Processing;

namespace SystemDot.Messaging.Channels.Building
{
    public class ProcessorBuilder<TIn, TOut>
    {
        readonly IMessageProcessor<TIn, TOut> processor;

        public ProcessorBuilder(IMessageProcessor<TIn, TOut> processor)
        {
            this.processor = processor;
        }

        public void ThenToEndPoint(IChannelEndPoint<TOut> endPoint)
        {
            processor.MessageProcessed += endPoint.InputMessage;
        }
    }
}