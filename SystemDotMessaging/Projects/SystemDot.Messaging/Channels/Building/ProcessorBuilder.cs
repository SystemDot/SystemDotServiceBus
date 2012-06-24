using SystemDot.Messaging.Channels.Messages;
using SystemDot.Messaging.Channels.Messages.Distribution;
using SystemDot.Messaging.Channels.Messages.Processing;
using SystemDot.Messaging.Configuration;
using SystemDot.Threading;

namespace SystemDot.Messaging.Channels.Building
{
    public class ProcessorBuilder<TIn, TOut>
    {
        readonly IMessageProcessor<TOut> processor;

        public ProcessorBuilder(IMessageProcessor<TOut> processor)
        {
            this.processor = processor;
        }

        public ProcessorBuilder<TOut, TOut> Pump()
        {
            var pump = new Pump<TOut>(MessagingEnvironment.GetComponent<IThreadPool>());
            this.processor.MessageProcessed += pump.InputMessage;

            return new ProcessorBuilder<TOut, TOut>(pump);
        }

        public ProcessorBuilder<TOut, TNextOut> ToProcessor<TNextOut>(IMessageProcessor<TOut, TNextOut> messageProcessor)
        {
            this.processor.MessageProcessed += messageProcessor.InputMessage;
            return new ProcessorBuilder<TOut, TNextOut>(messageProcessor);
        }

        public void ToEndPoint(IMessageInputter<TOut> endPoint)
        {
            processor.MessageProcessed += endPoint.InputMessage;
        }
    }
}