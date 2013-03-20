using System;

namespace SystemDot.Ioc
{
    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException(string message) : base(message)
        {
        }
    }
}