namespace SystemDot.Logging
{
    public interface ILoggingMechanism
    {
        void Info(string message);

        void Info(string message, params object[] args);

        void Error(string message);
    }
}