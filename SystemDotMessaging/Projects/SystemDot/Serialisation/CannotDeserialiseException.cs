using System;

namespace SystemDot.Serialisation
{
    public class CannotDeserialiseException : Exception
    {
        public CannotDeserialiseException(Exception inner) : 
            base("An object could not be deserialised from the byte array", inner)
        {
        }
    }
}