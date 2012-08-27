using System;

namespace SystemDot.Ioc
{
    public class ConcreteInstance
    {
        public Type ObjectType { get; private set; }
        public object ObjectInstance { get; private set; }
        public Func<object> ObjectFactory { get; set; }

        public ConcreteInstance(Type objectType)
        {
            this.ObjectType = objectType;
        }

        public ConcreteInstance(Func<object> objectFactory)
        {
            this.ObjectFactory = objectFactory;
        }

        public object SetObjectInstance(object instance)
        {
            this.ObjectInstance = instance;
            return instance;
        }
    }
}