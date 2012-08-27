using SystemDot.Ioc;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Transport;
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
            var pump = new Pump<TOut>(IocContainerLocator.Locate().Resolve<ITaskStarter>());
            this.processor.MessageProcessed += pump.InputMessage;

            return new ProcessorBuilder<TOut>(pump);
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