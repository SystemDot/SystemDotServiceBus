namespace SystemDot.Messaging.Configuration.Channels
{
    public class ProcessorBuilder<TIn, TOut>
    {
        readonly IMessageProcessor<TIn, TOut> processor;

        public ProcessorBuilder(IMessageProcessor<TIn, TOut> processor)
        {
            this.processor = processor;
        }

        public void ThenToEndPoint(IMessageEndPoint<TOut> endPoint)
        {
            processor.MessageProcessed += endPoint.InputMessage;
        }
    }
}