using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Messages.Pipelines
{
    public class ProcessorBuilder<TOut>
    {
        readonly IMessageProcessor<TOut> processor;

        public ProcessorBuilder(IMessageProcessor<TOut> processor)
        {
            this.processor = processor;
        }

        public ProcessorBuilder<TOut> Pump()
        {
            var pump = new Pump<TOut>(IocContainer.Resolve<ITaskStarter>());
            this.processor.MessageProcessed += pump.InputMessage;

            return new ProcessorBuilder<TOut>(pump);
        }

        public ProcessorBuilder<TOut> Pipe()
        {
            var pipe = new Pipe<TOut>();
            this.processor.MessageProcessed += pipe.InputMessage;

            return new ProcessorBuilder<TOut>(pipe);
        }

        public ProcessorBuilder<TNextOut> ToProcessor<TNextOut>(IMessageProcessor<TOut, TNextOut> messageProcessor)
        {
            this.processor.MessageProcessed += messageProcessor.InputMessage;
            return new ProcessorBuilder<TNextOut>(messageProcessor);
        }

        public void ToEndPoint(IMessageInputter<TOut> endPoint)
        {
            processor.MessageProcessed += endPoint.InputMessage;
        }
    }
}