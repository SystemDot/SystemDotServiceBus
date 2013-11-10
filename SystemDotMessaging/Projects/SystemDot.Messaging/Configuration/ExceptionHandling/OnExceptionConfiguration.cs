using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Configuration.ExceptionHandling
{
    public class OnExceptionConfiguration<TConfiguration> 
        where TConfiguration : IExceptionHandlingConfigurer
    {
        readonly TConfiguration configuration;

        public OnExceptionConfiguration(TConfiguration configuration)
        {
            Contract.Requires(!configuration.Equals(default(TConfiguration)));

            this.configuration = configuration;
        }

        public TConfiguration ContinueProcessingMessages()
        {
            configuration.SetContinueOnException();
            return configuration;
        }
    }
}