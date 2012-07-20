namespace SystemDot.Logging
{
    public interface ILoggingMechanism
    {
        void Info(string message);

        void Error(string message);
    }
}