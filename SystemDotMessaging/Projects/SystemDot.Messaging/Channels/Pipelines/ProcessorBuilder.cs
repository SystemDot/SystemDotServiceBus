using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Channels.Pipelines
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
            var pump = new Pump<TOut>(IocContainerLocator.Locate().Resolve<ITaskStarter>());
            //var pump = new Pipe<TOut>();
            this.processor.MessageProcessed += pump.InputMessage;

            return new ProcessorBuilder<TOut>(pump);
        }

        public ProcessorBuilder<TOut> Queue()
        {
            var queue = new Queue<TOut>(IocContainerLocator.Locate().Resolve<ITaskStarter>());
            //var queue = new Pipe<TOut>();
            this.processor.MessageProcessed += queue.InputMessage;

            return new ProcessorBuilder<TOut>(queue);
        }

        public ProcessorBuilder<TNextOut> ToConverter<TNextOut>(IMessageProcessor<TOut, TNextOut> messageProcessor)
        {
            this.processor.MessageProcessed += messageProcessor.InputMessage;
            return new ProcessorBuilder<TNextOut>(messageProcessor);
        }

        public ProcessorBuilder<TOut> ToProcessor(IMessageProcessor<TOut, TOut> messageProcessor)
        {
            this.processor.MessageProcessed += messageProcessor.InputMessage;
            return new ProcessorBuilder<TOut>(messageProcessor);
        }

        public ProcessorBuilder<TOut> ToProcessors(params IMessageProcessor<TOut, TOut>[] processors)
        {
            var builder = this;

            processors.ForEach(p => builder = builder.ToProcessor(p));

            return builder;
        }

        public void ToEndPoint(IMessageInputter<TOut> endPoint)
        {
            this.processor.MessageProcessed += endPoint.InputMessage;
        }
    }
}