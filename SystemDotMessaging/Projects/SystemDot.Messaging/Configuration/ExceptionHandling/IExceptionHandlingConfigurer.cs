using SystemDot.Messaging.ExceptionHandling;

namespace SystemDot.Messaging.Configuration.ExceptionHandling
{
    public interface IExceptionHandlingConfigurer
    {
        void SetContinueOnException();
    }
}