using SystemDot.Messaging.Channels.Messages.Distribution;
using SystemDot.Messaging.Channels.Messages.Processing;

namespace SystemDot.Messaging.Channels.Building
{
    public class PumpBuilder<T>
    {
        private readonly Pump<T> pump;

        public PumpBuilder(Pump<T> pump)
        {
            this.pump = pump;
        }

        public ProcessorBuilder<T, TOut> ToProcessor<TOut>(IMessageProcessor<T, TOut> messageProcessor)
        {
            this.pump.MessageProcessed += messageProcessor.InputMessage; 
            return new ProcessorBuilder<T, TOut>(messageProcessor);
        }
    }
}